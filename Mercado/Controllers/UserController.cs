using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mercado.Context;
using Mercado.Models;
using Mercado.ViewModels;

namespace Mercado.Controllers
{
    public class UserController : Controller
    {
        MercadoDbContext db = new MercadoDbContext();
        IPasswordHasher<User> passwordHasher = new PasswordHasher<User>();

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            bool emailExists = db.Users.Any(u => u.Email == vm.Email);
            if (emailExists)
            {
                ModelState.AddModelError(nameof(vm.Email), "This email is already registered");
                return View(vm);
            }

            var user = new User
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email
            };

            user.Password = passwordHasher.HashPassword(user, vm.Password);

            db.Users.Add(user);
            db.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = db.Users.SingleOrDefault(u => u.Email == vm.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(vm);
            }

            var result = passwordHasher.VerifyHashedPassword(user, user.Password, vm.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(vm);
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserFullName", $"{user.FirstName} {user.LastName}");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}