using Mercado.Context;
using Microsoft.EntityFrameworkCore;

namespace Mercado.Services
{
    public class ProductViewService : IProductViewService
    {
        private readonly MercadoDbContext _context;

        public ProductViewService(MercadoDbContext context)
        {
            _context = context;
        }

        public void IncrementViewCount(int productId)
        {
            _context.Products
                .Where(p => p.ProductId == productId).ExecuteUpdate(s => s.SetProperty(p => p.ViewCount, p => p.ViewCount + 1));
        }
    }
}