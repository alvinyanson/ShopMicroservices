using ProductCatalogService.Dtos;
using ProductCatalogService.Models;

namespace ProductCatalogService.Services.Contracts
{
    public interface ICartService
    {
        IEnumerable<Cart> GetCartItemsByOwnerId(string ownerId);
      
        Cart GetCartItemByIdAndOwnerId(int id, string ownerId);

        void AddCartItem(Cart cart);
       
        void UpdateCartItem(Cart cart);

        void RemoveCartItem(Cart cart);
       
        void Checkout(string ownerId);

        void SaveChanges();
    }
}
