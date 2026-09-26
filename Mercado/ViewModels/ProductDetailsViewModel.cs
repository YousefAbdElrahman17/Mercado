using Mercado.Models;

namespace Mercado.ViewModels
{
    public class ProductDetailsViewModel
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string? ImagePath { get; set; }
        public string? CategoryName { get; set; }
        public int ViewCount { get; set; }
        public IEnumerable<RecentlyViewedItem> RecentlyViewed { get; set; } = new List<RecentlyViewedItem>();
    }
}