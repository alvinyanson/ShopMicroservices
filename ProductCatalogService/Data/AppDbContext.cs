using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Models;

namespace ProductCatalogService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Carts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            Category[] seedCategories =
            [
                new() { Id = 1, Name = "Clothing & Apparel" },
                new() { Id = 2, Name = "Electronics" },
                new() { Id = 3, Name = "Home & Kitchen" },
                new() { Id = 4, Name = "Health & Beauty" },
                new() { Id = 5, Name = "Sports & Outdoors" },
                new() { Id = 6, Name = "Books & Media" },
                new() { Id = 7, Name = "Toys & Games" },
                new() { Id = 8, Name = "Automotive" },
                new() { Id = 9, Name = "Pets" },
                new() { Id = 10, Name = "Jewelry & Accessories" }
            ];


            Product[] seedProducts =
            [
                new() { Id = 1, Name = "Twin Lens Camera", Description = "Twin Lens Camera", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 1 },
                new() { Id = 2, Name = "Compact SLR Camera", Description = "Compact SLR Camera", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 2 },
                new() { Id = 3, Name = "Nikkormat SLR Camera", Description = "Nikkormat SLR Camera", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1900, CategoryId = 3 },
                new() { Id = 4, Name = "Compact Digital Camera", Description = "Compact Digital Camera", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1900, CategoryId = 4 },
                new() { Id = 5, Name = "Instamatic Camera", Description = "Instamatic Camera", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1900, CategoryId = 5 },
                new() { Id = 6, Name = "Tripod", Description = "Tripod", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 6 },
                new() { Id = 7, Name = "Vintage Folding Camera", Description = "Vintage Folding Camera", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 7 },
                new() { Id = 8, Name = "Camera Lens", Description = "Camera Lens", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 8 },
                new() { Id = 9, Name = "Instant Camera", Description = "Instant Camera", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 9 },
                new() { Id = 10, Name = "USB Cable", Description = "USB Cable", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1600, CategoryId = 10 },
                new() { Id = 11, Name = "Ethernet Cable", Description = "Ethernet Cable", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 100, CategoryId = 1 },
                new() { Id = 12, Name = "Clacky Keyboard", Description = "Clacky Keyboard", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 2 },
                new() { Id = 13, Name = "Hard Drive", Description = "Hard Drive", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 3 },
                new() { Id = 14, Name = "Gaming PC", Description = "Gaming PC", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 4 },
                new() { Id = 15, Name = "High Performance RAM", Description = "High Performance RAM", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 5 },
                new() { Id = 16, Name = "Curvy Monitor", Description = "Curvy Monitor", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 6 },
                new() { Id = 17, Name = "32-Inch Monitor", Description = "32-Inch Monitor", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1900, CategoryId = 7 },
                new() { Id = 18, Name = "Wireless Optical Mouse", Description = "Wireless Optical Mouse", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 8 },
                new() { Id = 19, Name = "Tablet", Description = "Tablet", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 9 },
                new() { Id = 20, Name = "Laptop", Description = "Laptop", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 10 },
                new() { Id = 21, Name = "Grey Fabric Sofa", Description = "Grey Fabric Sofa", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 1 },
                new() { Id = 22, Name = "Hi-Top Basketball Shoe", Description = "Hi-Top Basketball Shoe", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", Price = 250, CategoryId = 2 },
            ];



            builder.Entity<Category>().HasData(seedCategories);

            builder.Entity<Product>().HasData(seedProducts);
        }

    }
}
