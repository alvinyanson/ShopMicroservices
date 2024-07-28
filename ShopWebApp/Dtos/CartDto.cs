using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ShopWebApp.Dtos
{
    public class CartDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("product")]
        public ProductDto Product { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
