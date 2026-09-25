// Services/ProductViewService.cs
using Microsoft.EntityFrameworkCore;
using Mercado.Context;

namespace Mercado.Services
{
    public class ProductViewService : IProductViewService
    {
        private readonly MercadoDbContext _context;

        public ProductViewService(MercadoDbContext context)
        {
            _context = context;
        }

        public async Task IncrementViewCountAsync(int productId)
        {
            await _context.Products
                .Where(p => p.ProductId == productId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.ViewCount, p => p.ViewCount + 1));
        }
    }
}