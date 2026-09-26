// Models/Category.cs
using System.ComponentModel.DataAnnotations;

namespace Mercado.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public string? ImagePath {get; set;}

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}