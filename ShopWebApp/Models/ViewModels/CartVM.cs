using ShopWebApp.Dtos;

namespace ShopWebApp.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<ReadCartItemsDto> Items { get; set; }

        public double OrderTotal { get; set; }
    }
}
