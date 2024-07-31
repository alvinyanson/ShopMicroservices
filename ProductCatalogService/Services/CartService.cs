using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Dtos;
using ProductCatalogService.Models;
using ProductCatalogService.Services.Contracts;

namespace ProductCatalogService.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddCartItem(Cart cart)
        {
            _unitOfWork.Cart.Add(cart);
            SaveChanges();
        }

        public void Checkout(string ownerId)
        {
            var cartItems = GetCartItemsByOwnerId(ownerId);

            _unitOfWork.Cart.RemoveRange(cartItems);
            SaveChanges();
        }

        public Cart GetCartItemByIdAndOwnerId(int id, string ownerId)
        {
            return _unitOfWork.Cart.Get(u => u.Id == id && u.OwnerId == ownerId);
        }

        public IEnumerable<Cart> GetCartItemsByOwnerId(string ownerId)
        {
            return _unitOfWork.Cart.GetAll("Product").Where(u => u.OwnerId == ownerId);
        }

        public void RemoveCartItem(Cart cart)
        {
            _unitOfWork.Cart.Remove(cart);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _unitOfWork.Save();
        }

        public void UpdateCartItem(Cart cart)
        {
            _unitOfWork.Cart.Update(cart);
            SaveChanges();
        }
    }
}
