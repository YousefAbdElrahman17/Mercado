// Services/InventoryContextBuilder.cs
using Microsoft.EntityFrameworkCore;
using Mercado.Context;
using System.Text;

namespace Mercado.Services
{
    public class InventoryContextBuilder : IInventoryContextBuilder
    {
        private readonly MercadoDbContext _context;
        private const int LowStockThreshold = 5;

        public InventoryContextBuilder(MercadoDbContext context)
        {
            _context = context;
        }

        public async Task<string> BuildContextAsync()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            var sb = new StringBuilder();

            sb.AppendLine($"Total number of products: {products.Count}");
            sb.AppendLine($"Total inventory value (price x quantity): {products.Sum(p => p.Price * p.Quantity):0.00}");

            sb.AppendLine("\nAll products (Title | Category | Price | Quantity | Views):");
            foreach (var p in products.OrderByDescending(p => p.ViewCount))
                sb.AppendLine($"- {p.Title} | {p.Category?.Name} | {p.Price} | Qty: {p.Quantity} | Views: {p.ViewCount}");

            var lowStock = products.Where(p => p.Quantity <= LowStockThreshold).ToList();
            sb.AppendLine(lowStock.Any()
                ? $"\nLow-stock products (<= {LowStockThreshold}): {string.Join(", ", lowStock.Select(p => $"{p.Title} ({p.Quantity})"))}"
                : "\nNo low-stock products currently.");

            sb.AppendLine("\nNote: there is no real sales/purchase data in the system, so any analysis must rely only on available quantity and view count, not on sales figures.");

            return sb.ToString();
        }
    }
}