// Services/IDocumentChatService.cs
using Mercado.Models;

namespace Mercado.Services
{
    public interface IDocumentChatService
    {
        Task<ChatDocument?> GetActiveDocumentAsync(int userId);
        Task<ChatDocument> UploadDocumentAsync(IFormFile file, int userId);
        Task DeleteActiveDocumentAsync(int userId);
        Task<string> AskAsync(int userId, string question);
    }
}