// Services/InventoryChatService.cs
using Microsoft.EntityFrameworkCore;
using Mercado.Context;
using System.Text;

namespace Mercado.Services
{
    public class InventoryChatService : IInventoryChatService
    {
        private readonly MercadoDbContext _context;
        private readonly IGeminiService _gemini;

        public InventoryChatService(MercadoDbContext context, IGeminiService gemini)
        {
            _context = context;
            _gemini = gemini;
        }

        public async Task<string> AskAboutInventoryAsync(string question)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new { p.Title, Category = p.Category!.Name, p.Price, p.Quantity, p.ViewCount })
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Current products in stock:");
            foreach (var p in products)
                sb.AppendLine($"- {p.Title} | Category: {p.Category} | Price: {p.Price} | Quantity: {p.Quantity} | Views: {p.ViewCount}");

            return await _gemini.AskAsync(sb.ToString(), question);
        }
    }
}