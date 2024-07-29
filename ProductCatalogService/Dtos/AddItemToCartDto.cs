using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ProductCatalogService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProductCatalogService.Dtos
{
    public class AddItemToCartDto
    {
        [ValidateNever]
        public string OwnerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [DefaultValue(1)]
        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
        public int Quantity { get; set; }
    }
}
