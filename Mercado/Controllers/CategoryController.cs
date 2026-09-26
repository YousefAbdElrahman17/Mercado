using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mercado.Context;
using Mercado.Models;
using Mercado.ViewModels;
using Mercado.Filters;

namespace Mercado.Controllers
{
    [RequireLogin]
    public class CategoryController : Controller
    {
        MercadoDbContext db = new MercadoDbContext();

        [HttpGet]
        public IActionResult Index()
        {
            var vm = new CategoryListViewModel
            {
                Categories = db.Categories
                    .Select(c => new CategoryListItemViewModel
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name,
                        Description = c.Description,
                        ProductsCount = c.Products.Count
                    }).ToList()
            };

            foreach (var c in vm.Categories)
                c.IconClass = GetIconForCategory(c.Name);

            return View(vm);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var category = db.Categories.Include(c => c.Products).SingleOrDefault(c => c.CategoryId == id);
            if (category == null) return NotFound();

            var vm = new CategoryDetailsViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
                ProductTitles = category.Products.Select(p => p.Title).ToList(),
                IconClass = GetIconForCategory(category.Name)
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AddCategoryViewModel());
        }

        [HttpPost]
        public IActionResult Create(AddCategoryViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var category = new Category
            {
                Name = vm.Name,
                Description = vm.Description
            };

            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = db.Categories.SingleOrDefault(c => c.CategoryId == id);
            if (category == null) return NotFound();

            var vm = new EditCategoryViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(EditCategoryViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var category = db.Categories.SingleOrDefault(c => c.CategoryId == vm.CategoryId);
            if (category == null) return NotFound();

            category.Name = vm.Name;
            category.Description = vm.Description;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = db.Categories.Include(c => c.Products).SingleOrDefault(c => c.CategoryId == id);
            if (category == null) return NotFound();

            if (category.Products.Any())
            {
                TempData["Error"] = "You can't delete a category that still has products in it. Delete or reassign its products first.";
                return RedirectToAction("Index");
            }

            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private static string GetIconForCategory(string name) => name switch{
            "Electronics" => "ti-device-desktop",
            "Groceries" => "ti-shopping-cart",
            "Furniture" => "ti-armchair",
            "Clothing" => "ti-shirt",
            "Footwear" => "ti-shoe",
            "Toys & Games" => "ti-puzzle",
            "Books" => "ti-books",
            "Sports & Outdoors" => "ti-ball-football",
            "Beauty & Personal Care" => "ti-sparkles",
            "Health & Wellness" => "ti-heartbeat",
            "Automotive" => "ti-car",
            "Garden & Outdoor" => "ti-plant",
            "Kitchen & Dining" => "ti-tools-kitchen-2",
            "Office Supplies" => "ti-paperclip",
            "Pet Supplies" => "ti-paw",
            "Jewelry & Accessories" => "ti-diamond",
            "Musical Instruments" => "ti-music",
            "Home Appliances" => "ti-refrigerator",
            _ => "ti-tag"
        };
    }
}