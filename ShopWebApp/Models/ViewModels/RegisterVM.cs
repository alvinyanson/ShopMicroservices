using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShopWebApp.Models.ViewModels
{
    public class RegisterVM
    {
        public Register Register { get; set; }

        public IEnumerable<SelectListItem> Roles { get; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Admin", Value = "Admin" },
            new SelectListItem { Text = "Customer", Value = "Customer" }
        };

    }
}

