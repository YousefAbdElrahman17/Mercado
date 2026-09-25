// Services/DocumentChatService.cs
using Microsoft.EntityFrameworkCore;
using Mercado.Context;
using Mercado.Models;

namespace Mercado.Services
{
    public class DocumentChatService : IDocumentChatService
    {
        private readonly MercadoDbContext _context;
        private readonly IPdfTextExtractor _extractor;
        private readonly IGeminiService _gemini;

        public DocumentChatService(MercadoDbContext context, IPdfTextExtractor extractor, IGeminiService gemini)
        {
            _context = context;
            _extractor = extractor;
            _gemini = gemini;
        }

        public async Task<ChatDocument?> GetActiveDocumentAsync(int userId)
        {
            return await _context.ChatDocuments
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.UploadedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<ChatDocument> UploadDocumentAsync(IFormFile file, int userId)
        {
            await DeleteActiveDocumentAsync(userId);

            using var stream = file.OpenReadStream();
            var text = _extractor.ExtractText(stream);

            var document = new ChatDocument
            {
                FileName = file.FileName,
                ExtractedText = text,
                UserId = userId,
                UploadedAt = DateTime.UtcNow
            };

            _context.ChatDocuments.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task DeleteActiveDocumentAsync(int userId)
        {
            var existing = await _context.ChatDocuments.Where(d => d.UserId == userId).ToListAsync();
            if (existing.Any())
            {
                _context.ChatDocuments.RemoveRange(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> AskAsync(int userId, string question)
        {
            var doc = await GetActiveDocumentAsync(userId);
            if (doc == null)
                return "No document is uploaded yet. Please upload a PDF file first so I can answer questions based on it.";

            return await _gemini.AskAsync(doc.ExtractedText, question);
        }
    }
}