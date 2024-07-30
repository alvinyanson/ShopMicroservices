using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ShopWebApp.Dtos;

namespace ShopWebApp.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ValidateNever]
        public virtual ReadProductDto Product { get; set; }

        [DefaultValue(1)]
        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Quantity { get; set; }
    }
}
