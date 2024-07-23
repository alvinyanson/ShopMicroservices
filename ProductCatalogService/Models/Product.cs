
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalogService.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }


        [ValidateNever]
        public string? ImageUrl { get; set; }

        [Required]
        public double Price { get; set; }

        // explicit adding of annotation for foreign key
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation Property
        public Category Category { get; set; }
    }
}
