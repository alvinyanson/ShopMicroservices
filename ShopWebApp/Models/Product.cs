using System.Text.Json.Serialization;

namespace ShopWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public double Price { get; set; }

        public string CategoryId { get; set; }
    }
}
