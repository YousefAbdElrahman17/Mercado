using Microsoft.EntityFrameworkCore;
using Mercado.Models;
using Microsoft.AspNetCore.Identity;

namespace Mercado.Context
{
    public class MercadoDbContext:DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ChatDocument> ChatDocuments { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder(told u it's hidden)
            }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configuration (EF can detct this auto but this gives us more control)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatDocument>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Data Seeding

                            //  PasswordHasher -> make pass stored in db in symbols.
            const string demoPasswordHash = "AQAAAAIAAYagAAAAEBevwJzhWeoRtDQ35Sog1zjBjrgU4/xViWMZFQCllLaLJt8pEYY+Kl7E5U5iE+FzoA==";

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FirstName = "Ahmed",
                    LastName = "Mostafa",
                    Email = "ahmed@mercado.com",
                    Password = demoPasswordHash
                },
                new User
                {
                    UserId = 2,
                    FirstName = "Sara",
                    LastName = "Ali",
                    Email = "sara@mercado.com",
                    Password = demoPasswordHash
                }
            );

            // ---------- Categories (18) ----------
            var categories = new List<Category>
            {
                new() { CategoryId = 1, Name = "Electronics", Description = "Devices, gadgets and electronic accessories", ImagePath = "https://picsum.photos/seed/electronics/600/600" },
                new() { CategoryId = 2, Name = "Groceries", Description = "Everyday food and pantry items", ImagePath = "https://picsum.photos/seed/groceries/600/600" },
                new() { CategoryId = 3, Name = "Furniture", Description = "Home and office furniture", ImagePath = "https://picsum.photos/seed/furniture/600/600" },
                new() { CategoryId = 4, Name = "Clothing", Description = "Men's, women's and kids' clothing", ImagePath = "https://picsum.photos/seed/clothing/600/600" },
                new() { CategoryId = 5, Name = "Footwear", Description = "Shoes and boots for all ages", ImagePath = "https://picsum.photos/seed/footwear/600/600" },
                new() { CategoryId = 6, Name = "Toys & Games", Description = "Toys, board games and puzzles", ImagePath = "https://picsum.photos/seed/toysgames/600/600" },
                new() { CategoryId = 7, Name = "Books", Description = "Fiction, non-fiction and educational books", ImagePath = "https://picsum.photos/seed/books/600/600" },
                new() { CategoryId = 8, Name = "Sports & Outdoors", Description = "Sporting goods and outdoor gear", ImagePath = "https://picsum.photos/seed/sportsoutdoors/600/600" },
                new() { CategoryId = 9, Name = "Beauty & Personal Care", Description = "Skincare, haircare and personal care products", ImagePath = "https://picsum.photos/seed/beauty/600/600" },
                new() { CategoryId = 10, Name = "Health & Wellness", Description = "Health devices and wellness supplements", ImagePath = "https://picsum.photos/seed/health/600/600" },
                new() { CategoryId = 11, Name = "Automotive", Description = "Car accessories and maintenance products", ImagePath = "https://picsum.photos/seed/automotive/600/600" },
                new() { CategoryId = 12, Name = "Garden & Outdoor", Description = "Gardening tools and outdoor living products", ImagePath = "https://picsum.photos/seed/garden/600/600" },
                new() { CategoryId = 13, Name = "Kitchen & Dining", Description = "Cookware, utensils and dining sets", ImagePath = "https://picsum.photos/seed/kitchen/600/600" },
                new() { CategoryId = 14, Name = "Office Supplies", Description = "Stationery and office equipment", ImagePath = "https://picsum.photos/seed/office/600/600" },
                new() { CategoryId = 15, Name = "Pet Supplies", Description = "Food and accessories for pets", ImagePath = "https://picsum.photos/seed/pets/600/600" },
                new() { CategoryId = 16, Name = "Jewelry & Accessories", Description = "Jewelry, watches and fashion accessories", ImagePath = "https://picsum.photos/seed/jewelry/600/600" },
                new() { CategoryId = 17, Name = "Musical Instruments", Description = "Instruments and music accessories", ImagePath = "https://picsum.photos/seed/music/600/600" },
                new() { CategoryId = 18, Name = "Home Appliances", Description = "Electrical appliances for the home", ImagePath = "https://picsum.photos/seed/appliances/600/600" }
            };

            var products = new List<Product>
            {
                new() { ProductId = 1, Title = "Wireless Bluetooth Headphones", Description = "Over-ear headphones with noise cancellation", Price = 45.99m, Quantity = 40, CategoryId = 1, ImagePath = "https://picsum.photos/seed/product1/500/500" },
                new() { ProductId = 2, Title = "27-inch 4K Monitor", Description = "Ultra HD monitor for work and gaming", Price = 249.99m, Quantity = 3, CategoryId = 1, ImagePath = "https://picsum.photos/seed/product2/500/500" },
                new() { ProductId = 3, Title = "Mechanical Keyboard", Description = "RGB backlit mechanical keyboard", Price = 59.99m, Quantity = 25, CategoryId = 1, ImagePath = "https://picsum.photos/seed/product3/500/500" },
                new() { ProductId = 4, Title = "USB-C Charging Cable", Description = "1.5m fast-charging cable", Price = 8.99m, Quantity = 120, CategoryId = 1, ImagePath = "https://picsum.photos/seed/product4/500/500" },

                new() { ProductId = 5, Title = "Organic Olive Oil 1L", Description = "Cold-pressed extra virgin olive oil", Price = 12.50m, Quantity = 60, CategoryId = 2, ImagePath = "https://picsum.photos/seed/product5/500/500" },
                new() { ProductId = 6, Title = "Basmati Rice 5kg", Description = "Premium long-grain basmati rice", Price = 9.75m, Quantity = 4, CategoryId = 2, ImagePath = "https://picsum.photos/seed/product6/500/500" },
                new() { ProductId = 7, Title = "Roasted Coffee Beans 1kg", Description = "Medium roast arabica coffee beans", Price = 14.00m, Quantity = 30, CategoryId = 2, ImagePath = "https://picsum.photos/seed/product7/500/500" },
                new() { ProductId = 8, Title = "Honey Jar 500g", Description = "Natural raw honey", Price = 7.25m, Quantity = 55, CategoryId = 2, ImagePath = "https://picsum.photos/seed/product8/500/500" },

                new() { ProductId = 9, Title = "Wooden Dining Table", Description = "6-seater solid wood dining table", Price = 320.00m, Quantity = 5, CategoryId = 3, ImagePath = "https://picsum.photos/seed/product9/500/500" },
                new() { ProductId = 10, Title = "Ergonomic Office Chair", Description = "Adjustable chair with lumbar support", Price = 149.99m, Quantity = 18, CategoryId = 3, ImagePath = "https://picsum.photos/seed/product10/500/500" },
                new() { ProductId = 11, Title = "5-Tier Bookshelf", Description = "Modern open bookshelf", Price = 89.50m, Quantity = 12, CategoryId = 3, ImagePath = "https://picsum.photos/seed/product11/500/500" },
                new() { ProductId = 12, Title = "3-Seater Sofa", Description = "Fabric upholstered sofa", Price = 499.00m, Quantity = 2, CategoryId = 3, ImagePath = "https://picsum.photos/seed/product12/500/500" },

                new() { ProductId = 13, Title = "Men's Cotton T-Shirt", Description = "100% cotton crew neck t-shirt", Price = 12.99m, Quantity = 100, CategoryId = 4, ImagePath = "https://picsum.photos/seed/product13/500/500" },
                new() { ProductId = 14, Title = "Women's Denim Jacket", Description = "Classic fit denim jacket", Price = 45.00m, Quantity = 22, CategoryId = 4, ImagePath = "https://picsum.photos/seed/product14/500/500" },
                new() { ProductId = 15, Title = "Kids Hoodie", Description = "Soft fleece hoodie for kids", Price = 18.50m, Quantity = 40, CategoryId = 4, ImagePath = "https://picsum.photos/seed/product15/500/500" },
                new() { ProductId = 16, Title = "Winter Wool Coat", Description = "Warm wool blend winter coat", Price = 79.99m, Quantity = 3, CategoryId = 4, ImagePath = "https://picsum.photos/seed/product16/500/500" },

                new() { ProductId = 17, Title = "Running Sneakers", Description = "Lightweight breathable running shoes", Price = 65.00m, Quantity = 30, CategoryId = 5, ImagePath = "https://picsum.photos/seed/product17/500/500" },
                new() { ProductId = 18, Title = "Leather Formal Shoes", Description = "Genuine leather formal shoes", Price = 89.99m, Quantity = 15, CategoryId = 5, ImagePath = "https://picsum.photos/seed/product18/500/500" },
                new() { ProductId = 19, Title = "Kids Sandals", Description = "Comfortable summer sandals", Price = 15.00m, Quantity = 50, CategoryId = 5, ImagePath = "https://picsum.photos/seed/product19/500/500" },
                new() { ProductId = 20, Title = "Hiking Boots", Description = "Waterproof hiking boots", Price = 95.00m, Quantity = 4, CategoryId = 5, ImagePath = "https://picsum.photos/seed/product20/500/500" },

                new() { ProductId = 21, Title = "Building Blocks Set", Description = "300-piece creative building blocks", Price = 25.00m, Quantity = 35, CategoryId = 6, ImagePath = "https://picsum.photos/seed/product21/500/500" },
                new() { ProductId = 22, Title = "Remote Control Car", Description = "High-speed RC car", Price = 39.99m, Quantity = 20, CategoryId = 6, ImagePath = "https://picsum.photos/seed/product22/500/500" },
                new() { ProductId = 23, Title = "1000-Piece Puzzle", Description = "Scenic landscape jigsaw puzzle", Price = 14.50m, Quantity = 28, CategoryId = 6, ImagePath = "https://picsum.photos/seed/product23/500/500" },
                new() { ProductId = 24, Title = "Family Board Game Pack", Description = "Set of 3 classic family board games", Price = 29.99m, Quantity = 5, CategoryId = 6, ImagePath = "https://picsum.photos/seed/product24/500/500" },

                new() { ProductId = 25, Title = "Introduction to Algorithms", Description = "Computer science reference book", Price = 55.00m, Quantity = 12, CategoryId = 7, ImagePath = "https://picsum.photos/seed/product25/500/500" },
                new() { ProductId = 26, Title = "Learning C# Programming", Description = "Beginner-friendly C# guide", Price = 39.99m, Quantity = 20, CategoryId = 7, ImagePath = "https://picsum.photos/seed/product26/500/500" },
                new() { ProductId = 27, Title = "The Art of Cooking", Description = "Illustrated cookbook", Price = 22.00m, Quantity = 18, CategoryId = 7, ImagePath = "https://picsum.photos/seed/product27/500/500" },
                new() { ProductId = 28, Title = "History of Egypt", Description = "A comprehensive historical overview", Price = 18.50m, Quantity = 3, CategoryId = 7, ImagePath = "https://picsum.photos/seed/product28/500/500" },

                new() { ProductId = 29, Title = "Yoga Mat", Description = "Non-slip exercise yoga mat", Price = 19.99m, Quantity = 45, CategoryId = 8, ImagePath = "https://picsum.photos/seed/product29/500/500" },
                new() { ProductId = 30, Title = "Adjustable Dumbbells Set", Description = "5-25kg adjustable dumbbells", Price = 89.00m, Quantity = 10, CategoryId = 8, ImagePath = "https://picsum.photos/seed/product30/500/500" },
                new() { ProductId = 31, Title = "4-Person Camping Tent", Description = "Waterproof family camping tent", Price = 129.99m, Quantity = 6, CategoryId = 8, ImagePath = "https://picsum.photos/seed/product31/500/500" },
                new() { ProductId = 32, Title = "Soccer Ball", Description = "Official size 5 soccer ball", Price = 14.99m, Quantity = 60, CategoryId = 8, ImagePath = "https://picsum.photos/seed/product32/500/500" },

                new() { ProductId = 33, Title = "Facial Cleanser 150ml", Description = "Gentle daily facial cleanser", Price = 9.99m, Quantity = 40, CategoryId = 9, ImagePath = "https://picsum.photos/seed/product33/500/500" },
                new() { ProductId = 34, Title = "Shampoo & Conditioner Set", Description = "Nourishing hair care duo", Price = 15.50m, Quantity = 35, CategoryId = 9, ImagePath = "https://picsum.photos/seed/product34/500/500" },
                new() { ProductId = 35, Title = "Electric Hair Dryer", Description = "Fast-drying ionic hair dryer", Price = 29.99m, Quantity = 5, CategoryId = 9, ImagePath = "https://picsum.photos/seed/product35/500/500" },
                new() { ProductId = 36, Title = "Perfume 100ml", Description = "Long-lasting eau de parfum", Price = 45.00m, Quantity = 20, CategoryId = 9, ImagePath = "https://picsum.photos/seed/product36/500/500" },
                
                new() { ProductId = 37, Title = "Digital Blood Pressure Monitor", Description = "Automatic arm blood pressure monitor", Price = 34.99m, Quantity = 15, CategoryId = 10, ImagePath = "https://picsum.photos/seed/product37/500/500" },
                new() { ProductId = 38, Title = "Vitamin C Supplements", Description = "Immune support tablets, 90 count", Price = 12.00m, Quantity = 50, CategoryId = 10, ImagePath = "https://picsum.photos/seed/product38/500/500" },
                new() { ProductId = 39, Title = "First Aid Kit", Description = "Complete home first aid kit", Price = 18.00m, Quantity = 25, CategoryId = 10, ImagePath = "https://picsum.photos/seed/product39/500/500" },
                new() { ProductId = 40, Title = "Electric Toothbrush", Description = "Rechargeable sonic toothbrush", Price = 22.50m, Quantity = 4, CategoryId = 10, ImagePath = "https://picsum.photos/seed/product40/500/500" },

                new() { ProductId = 41, Title = "Car Phone Mount", Description = "Dashboard and windshield phone mount", Price = 11.99m, Quantity = 60, CategoryId = 11, ImagePath = "https://picsum.photos/seed/product41/500/500" },
                new() { ProductId = 42, Title = "Motor Oil 5L", Description = "Synthetic engine motor oil", Price = 28.00m, Quantity = 30, CategoryId = 11, ImagePath = "https://picsum.photos/seed/product42/500/500" },
                new() { ProductId = 43, Title = "Car Vacuum Cleaner", Description = "Portable handheld car vacuum", Price = 39.99m, Quantity = 8, CategoryId = 11, ImagePath = "https://picsum.photos/seed/product43/500/500" },
                new() { ProductId = 44, Title = "Tire Pressure Gauge", Description = "Digital tire pressure gauge", Price = 7.50m, Quantity = 45, CategoryId = 11, ImagePath = "https://picsum.photos/seed/product44/500/500" },

                new() { ProductId = 45, Title = "Garden Hose 20m", Description = "Expandable flexible garden hose", Price = 24.99m, Quantity = 20, CategoryId = 12, ImagePath = "https://picsum.photos/seed/product45/500/500" },
                new() { ProductId = 46, Title = "Manual Lawn Mower", Description = "Push-reel lawn mower", Price = 89.99m, Quantity = 3, CategoryId = 12, ImagePath = "https://picsum.photos/seed/product46/500/500" },
                new() { ProductId = 47, Title = "Flower Pot Set", Description = "Set of 5 ceramic flower pots", Price = 15.00m, Quantity = 40, CategoryId = 12, ImagePath = "https://picsum.photos/seed/product47/500/500" },
                new() { ProductId = 48, Title = "Outdoor Solar Lights", Description = "Pack of 6 solar garden lights", Price = 19.99m, Quantity = 30, CategoryId = 12, ImagePath = "https://picsum.photos/seed/product48/500/500" },

                new() { ProductId = 49, Title = "Non-Stick Frying Pan", Description = "28cm non-stick frying pan", Price = 22.00m, Quantity = 35, CategoryId = 13, ImagePath = "https://picsum.photos/seed/product49/500/500" },
                new() { ProductId = 50, Title = "6-Piece Knife Set", Description = "Stainless steel kitchen knife set", Price = 34.99m, Quantity = 20, CategoryId = 13, ImagePath = "https://picsum.photos/seed/product50/500/500" },
                new() { ProductId = 51, Title = "Blender 700W", Description = "High-speed countertop blender", Price = 45.00m, Quantity = 12, CategoryId = 13, ImagePath = "https://picsum.photos/seed/product51/500/500" },
                new() { ProductId = 52, Title = "12-Piece Dinner Plate Set", Description = "Porcelain dinner plate set", Price = 29.99m, Quantity = 5, CategoryId = 13, ImagePath = "https://picsum.photos/seed/product52/500/500" },

                new() { ProductId = 53, Title = "A4 Paper Ream", Description = "500 sheets, 80gsm A4 paper", Price = 5.99m, Quantity = 100, CategoryId = 14, ImagePath = "https://picsum.photos/seed/product53/500/500" },
                new() { ProductId = 54, Title = "Ballpoint Pens 10-Pack", Description = "Smooth-writing blue ink pens", Price = 3.50m, Quantity = 150, CategoryId = 14, ImagePath = "https://picsum.photos/seed/product54/500/500" },
                new() { ProductId = 55, Title = "Desk Organizer", Description = "Multi-compartment desk organizer", Price = 12.99m, Quantity = 25, CategoryId = 14, ImagePath = "https://picsum.photos/seed/product55/500/500" },
                new() { ProductId = 56, Title = "Heavy Duty Stapler", Description = "High-capacity office stapler", Price = 8.99m, Quantity = 30, CategoryId = 14, ImagePath = "https://picsum.photos/seed/product56/500/500" },

                new() { ProductId = 57, Title = "Dog Food 10kg", Description = "Balanced adult dog dry food", Price = 32.00m, Quantity = 20, CategoryId = 15, ImagePath = "https://picsum.photos/seed/product57/500/500" },
                new() { ProductId = 58, Title = "Cat Litter Box", Description = "Covered cat litter box", Price = 18.50m, Quantity = 15, CategoryId = 15, ImagePath = "https://picsum.photos/seed/product58/500/500" },
                new() { ProductId = 59, Title = "Pet Grooming Brush", Description = "De-shedding grooming brush", Price = 9.99m, Quantity = 40, CategoryId = 15, ImagePath = "https://picsum.photos/seed/product59/500/500" },
                new() { ProductId = 60, Title = "Medium Bird Cage", Description = "Spacious cage for small birds", Price = 29.99m, Quantity = 4, CategoryId = 15, ImagePath = "https://picsum.photos/seed/product60/500/500" },

                new() { ProductId = 61, Title = "Silver Necklace", Description = "Sterling silver pendant necklace", Price = 55.00m, Quantity = 10, CategoryId = 16, ImagePath = "https://picsum.photos/seed/product61/500/500" },
                new() { ProductId = 62, Title = "Leather Wallet", Description = "Genuine leather bifold wallet", Price = 25.00m, Quantity = 30, CategoryId = 16, ImagePath = "https://picsum.photos/seed/product62/500/500" },
                new() { ProductId = 63, Title = "Classic Sunglasses", Description = "UV-protection classic sunglasses", Price = 19.99m, Quantity = 25, CategoryId = 16, ImagePath = "https://picsum.photos/seed/product63/500/500" },
                new() { ProductId = 64, Title = "Analog Wrist Watch", Description = "Stainless steel analog watch", Price = 65.00m, Quantity = 3, CategoryId = 16, ImagePath = "https://picsum.photos/seed/product64/500/500" },

                new() { ProductId = 65, Title = "Acoustic Guitar", Description = "Full-size acoustic guitar", Price = 149.99m, Quantity = 8, CategoryId = 17, ImagePath = "https://picsum.photos/seed/product65/500/500" },
                new() { ProductId = 66, Title = "Digital Piano Keyboard", Description = "61-key digital keyboard", Price = 299.00m, Quantity = 4, CategoryId = 17, ImagePath = "https://picsum.photos/seed/product66/500/500" },
                new() { ProductId = 67, Title = "Violin Beginner Set", Description = "Full-size violin with case and bow", Price = 89.99m, Quantity = 6, CategoryId = 17, ImagePath = "https://picsum.photos/seed/product67/500/500" },
                new() { ProductId = 68, Title = "Drum Practice Pad", Description = "Silent drum practice pad", Price = 24.99m, Quantity = 15, CategoryId = 17, ImagePath = "https://picsum.photos/seed/product68/500/500" },

                new() { ProductId = 69, Title = "Microwave Oven 700W", Description = "Compact countertop microwave", Price = 79.99m, Quantity = 10, CategoryId = 18, ImagePath = "https://picsum.photos/seed/product69/500/500" },
                new() { ProductId = 70, Title = "Bagless Vacuum Cleaner", Description = "Powerful bagless vacuum cleaner", Price = 99.99m, Quantity = 7, CategoryId = 18, ImagePath = "https://picsum.photos/seed/product70/500/500" },
                new() { ProductId = 71, Title = "Electric Kettle 1.7L", Description = "Fast-boil electric kettle", Price = 19.99m, Quantity = 25, CategoryId = 18, ImagePath = "https://picsum.photos/seed/product71/500/500" },
                new() { ProductId = 72, Title = "Air Fryer 4L", Description = "Oil-free air fryer", Price = 65.00m, Quantity = 5, CategoryId = 18, ImagePath = "https://picsum.photos/seed/product72/500/500" }
                
        };

            modelBuilder.Entity<Category>().HasData(categories);
            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
