﻿using System.Text.Json.Serialization;

namespace ShopWebApp.Dtos
{
    public class ReadProductDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

    }
}