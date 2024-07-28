using ShopWebApp.Dtos;

namespace ShopWebApp.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<CartDto> Items { get; set; }

        public double OrderTotal { get; set; }
    }
}
