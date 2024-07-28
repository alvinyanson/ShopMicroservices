using Microsoft.AspNetCore.Mvc.Rendering;
using ShopWebApp.Dtos;

namespace ShopWebApp.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
