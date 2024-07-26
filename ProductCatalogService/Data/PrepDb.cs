using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ProductCatalogService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope()) 
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"Could not run migrations: {ex.Message}");
                }
            }


            Console.WriteLine($"isProd: {isProd}");


            //if(!context.Platforms.Any())
            //{
            //    Console.WriteLine("Seeding data...");

            //    context.Platforms.AddRange(
            //        new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
            //        new Platform() { Name = "Sql Server Database", Publisher = "Microsoft", Cost = "Free" },
            //        new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            //    );

            //    context.SaveChanges();
            //}
            //else
            //{
            //    Console.WriteLine("Data already available...");
            //}
        }
    }
}
