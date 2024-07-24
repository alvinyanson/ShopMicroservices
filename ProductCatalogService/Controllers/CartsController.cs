using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Dtos;
using ProductCatalogService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CartsController(
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<string> GetCartItems()
        {
            var cartItems = _unitOfWork.Cart.GetAll();
         
            return Ok(new { success = true, message = "Cart items retrieved!", result = cartItems });
        }

        [HttpPost]
        public ActionResult<string> AddItemToCart(AddToCartDto addToCartDto)
        {
            string? ownerId = "d947e794-b0e3-49e3-9a38-7ed8b327883f"; // TODO retrieve from auth header

            Cart cartFromDb = _unitOfWork.Cart.Get(u => u.OwnerId == ownerId && u.ProductId == addToCartDto.ProductId);

            if (cartFromDb != null)
            {
                cartFromDb.Quantity += addToCartDto.Quantity;

                _unitOfWork.Cart.Update(cartFromDb);

                _unitOfWork.Save();

                return Ok(new { success = true, message = "Item updated from cart!" });
            }
            else
            {
                var result = _mapper.Map<Cart>(addToCartDto);

                _unitOfWork.Cart.Add(result);

                _unitOfWork.Save();

                return Ok(new { success = true, message = "Item added to cart!" });
            }

        }

        [HttpDelete("{id}")]
        public ActionResult<string> DeleteItemFrom(int id)
        {
            var cartFromDb = _unitOfWork.Cart.Get(u => u.Id == id);

            if (cartFromDb == null)
            {
                return NotFound(new { success = false, message = "Item cannot does not exist from cart!" });
            }

            _unitOfWork.Cart.Remove(cartFromDb);
            
            _unitOfWork.Save();
            
            return Ok(new { success = true, message = "Item deleted from cart!" });
        }
    }
}
