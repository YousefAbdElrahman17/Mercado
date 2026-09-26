using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mercado.Context;
using Mercado.Models;
using Mercado.ViewModels;
using Mercado.Helpers;
using Mercado.Services;
using Mercado.Filters;

namespace Mercado.Controllers
{
    [RequireLogin]
    public class ProductController : Controller
    {
        MercadoDbContext db = new MercadoDbContext();
        private readonly IProductViewService _viewService;
        private readonly IWebHostEnvironment _env;

        public ProductController(IProductViewService viewService, IWebHostEnvironment env)
        {
            _viewService = viewService;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vm = new ProductListViewModel
            {
                Products = db.Products
                    .Include(p => p.Category)
                    .OrderByDescending(p => p.ProductId)
                    .Select(p => new ProductListItemViewModel
                    {
                        ProductId = p.ProductId,
                        Title = p.Title,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        ImagePath = p.ImagePath,
                        CategoryName = p.Category.Name,
                        ViewCount = p.ViewCount
                    }).ToList()
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = db.Products.Include(p => p.Category).SingleOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();

            _viewService.IncrementViewCount(id);
            RecentlyViewedTracker.Track(HttpContext.Session, "Product", product.ProductId, product.Title);

            var vm = new ProductDetailsViewModel
            {
                ProductId = product.ProductId,
                Title = product.Title,
                Price = product.Price,
                Description = product.Description,
                Quantity = product.Quantity,
                ImagePath = product.ImagePath,
                CategoryName = product.Category?.Name,
                ViewCount = product.ViewCount + 1,
                RecentlyViewed = RecentlyViewedTracker.GetRecent(HttpContext.Session, "Product")
                    .Where(x => x.Id != id)
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new AddProductViewModel
            {
                Categories = GetCategorySelectList()
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(AddProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = GetCategorySelectList();
                return View(vm);
            }

            var product = new Product
            {
                Title = vm.Title,
                Price = vm.Price,
                Description = vm.Description,
                Quantity = vm.Quantity,
                CategoryId = vm.CategoryId
            };

            if (vm.ImageFile != null)
                product.ImagePath = SaveImage(vm.ImageFile);

            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = db.Products.SingleOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();

            var vm = new EditProductViewModel
            {
                ProductId = product.ProductId,Title = product.Title,
                Price = product.Price,
                Description = product.Description,
                Quantity = product.Quantity,
                CurrentImagePath = product.ImagePath,
                CategoryId = product.CategoryId,
                Categories = GetCategorySelectList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(EditProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = GetCategorySelectList();
                return View(vm);
            }

            var product = db.Products.SingleOrDefault(p => p.ProductId == vm.ProductId);
            if (product == null) return NotFound();

            product.Title = vm.Title;
            product.Price = vm.Price;
            product.Description = vm.Description;
            product.Quantity = vm.Quantity;
            product.CategoryId = vm.CategoryId;

            if (vm.ImageFile != null)
                product.ImagePath = SaveImage(vm.ImageFile);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = db.Products.SingleOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();

            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetCategorySelectList()
        {
            return db.Categories
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                .ToList();
        }

        private string SaveImage(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"/images/products/{fileName}";
        }
    }
}