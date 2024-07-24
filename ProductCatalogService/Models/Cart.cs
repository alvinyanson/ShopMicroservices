using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProductCatalogService.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ValidateNever]
        public virtual Product Product { get; set; }

        [DefaultValue(0)]
        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
        public int Quantity { get; set; }
    }
}
