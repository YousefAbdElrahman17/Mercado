namespace Mercado.ViewModels
{
    public class CategoryDetailsViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string> ProductTitles { get; set; } = new();
        public string IconClass { get; set; } = "ti-tag";
    }
}