using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShopWebApp.Dtos
{
    public class AddItemToCartDto
    {
        [Required]
        public int ProductId { get; set; }

        [DefaultValue(0)]
        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Quantity { get; set; }
    }
}
