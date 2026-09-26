using Microsoft.AspNetCore.Mvc;
using Mercado.Context;
using Mercado.Filters;
using Mercado.ViewModels;

namespace Mercado.Controllers
{
    [RequireLogin]
    public class HomeController : Controller
    {
        MercadoDbContext db = new MercadoDbContext();
        private const int LowStockThreshold = 5;

        public IActionResult Index()
        {
            var vm = new HomeViewModel
            {
                UserFullName = HttpContext.Session.GetString("UserFullName") ?? "",
                ProductsCount = db.Products.Count(),
                CategoriesCount = db.Categories.Count(),
                LowStockCount = db.Products.Count(p => p.Quantity <= LowStockThreshold)
            };

            return View(vm);
        }
    }
}