using Mercado.Models;
using Mercado.Services;
using Mercado.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Mercado
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(55);
            });

            if(args.Length>0 && args[0] == "hash")
            {
                var hasher = new PasswordHasher<User>();
                Console.WriteLine(hasher.HashPassword(null!,"Test@1234"));
                return;
            }

            var app = builder.Build();

            //////----------------------------------------------------------------
            // ---------- Database ----------
            builder.Services.AddDbContext<MercadoDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ---------- Session ----------
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // ---------- Auth ----------
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // ---------- Product view tracking ----------
            builder.Services.AddScoped<IProductViewService, ProductViewService>();

            // ---------- AI services (Chatbot + Insights Dashboard) ----------
            builder.Services.AddHttpClient<IGeminiService, GeminiService>();
            builder.Services.AddScoped<IInventoryChatService, InventoryChatService>();
            builder.Services.AddScoped<IInventoryContextBuilder, InventoryContextBuilder>();
            builder.Services.AddScoped<IInventoryInsightsService, InventoryInsightsService>();
            builder.Services.AddScoped<IPdfTextExtractor, PdfTextExtractor>();
            builder.Services.AddScoped<IDocumentChatService, DocumentChatService>();
            /// --------------------------------------------------------------------
             
            
            //Run both
            app.Map("/middleware-test", middlewareApp =>
            {
                middlewareApp.Use(async (HttpContext, next) =>
                {
                    await HttpContext.Response.WriteAsync("1) hello from Middleware 1\n");
                    await next();
                    await HttpContext.Response.WriteAsync("5) hello from Middleware 5");
                });

                middlewareApp.Use(async (HttpContext, next) =>
                {
                    await HttpContext.Response.WriteAsync("2) hello from Middleware 2\n");
                    await next();
                    await HttpContext.Response.WriteAsync("4) hello from Middleware 4\n");
                });

                middlewareApp.Run(async (HttpContext) =>
                {
                    await HttpContext.Response.WriteAsync("3) hello from Middleware 3\n");
                });
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}