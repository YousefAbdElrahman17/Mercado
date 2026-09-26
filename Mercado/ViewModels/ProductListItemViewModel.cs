namespace Mercado.ViewModels
{
    public class ProductListItemViewModel
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImagePath { get; set; }
        public string CategoryName { get; set; }
        public int ViewCount { get; set; }

        public bool IsLowStock => Quantity <= 5;
    }
}