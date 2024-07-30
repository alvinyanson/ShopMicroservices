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
                new() { Id = 1, Name = "Spiky Cactus", Description = "Spiky Cactus", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/78/charles-deluvio-695736-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 3 },
                new() { Id = 2, Name = "Tulip Pot", Description = "Tulip Pot", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/14/natalia-y-345738-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 3 },
                new() { Id = 3, Name = "Hanging Plant", Description = "Hanging Plant", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/5b/alex-rodriguez-santibanez-200278-unsplash__preview.jpg?w=200&h=200", Price = 1900, CategoryId = 3 },
                new() { Id = 4, Name = "Aloe Vera", Description = "Aloe Vera", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/29/silvia-agrasar-227575-unsplash__preview.jpg?w=200&h=200", Price = 1900, CategoryId = 3 },
                new() { Id = 5, Name = "Fern Blechnum Gibbum", Description = "Fern Blechnum Gibbum", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/6d/caleb-george-536388-unsplash__preview.jpg?w=200&h=200", Price = 1900, CategoryId = 3 },
                new() { Id = 6, Name = "Assorted Indoor Succulents", Description = "Assorted Indoor Succulents", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/81/annie-spratt-78044-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 3 },
                new() { Id = 7, Name = "Orchid", Description = "Orchid", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/88/zoltan-kovacs-642412-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 3 },
                new() { Id = 8, Name = "Bonsai Tree", Description = "Bonsai Tree", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/f3/mark-tegethoff-667351-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 3 },
                new() { Id = 9, Name = "Guardian Lion Statue", Description = "Guardian Lion Statue", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/44/vincent-liu-525429-unsplash__preview.jpg?w=200&h=200", Price = 1000, CategoryId = 3 },
                new() { Id = 10, Name = "Hand Trowel", Description = "Hand Trowel", ImageUrl = "https://readonlydemo.vendure.io/assets/preview/7d/neslihan-gunaydin-3493-unsplash__preview.jpg?w=200&h=200", Price = 1600, CategoryId = 3 },
            ];



            builder.Entity<Category>().HasData(seedCategories);

            builder.Entity<Product>().HasData(seedProducts);
        }

    }
}
