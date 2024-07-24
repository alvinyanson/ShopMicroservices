using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Dtos;
using ProductCatalogService.Models;
using ProductCatalogService.SyncDataServices.Http;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommandDataClient _commandDataClient;

        public CartsController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            ICommandDataClient commandDataClient)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetCartItems()
        {
            string? ownerId = await GetUserIdFromHeader();

            if (string.IsNullOrEmpty(ownerId))
            {
                return Unauthorized();
            }

            var cartItems = _unitOfWork.Cart.GetAll().Where(u => u.OwnerId == ownerId);
         
            return Ok(new { success = true, message = "Cart items retrieved!", result = cartItems });
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddItemToCart(AddToCartDto addToCartDto)
        {
            string? ownerId = await GetUserIdFromHeader();

            if (string.IsNullOrEmpty(ownerId))
            {
                return Unauthorized();
            }

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
        public async Task<ActionResult<string>> DeleteItemFrom(int id)
        {
            string? ownerId = await GetUserIdFromHeader();

            if (string.IsNullOrEmpty(ownerId))
            {
                return Unauthorized();
            }

            var cartFromDb = _unitOfWork.Cart.Get(u => u.Id == id);

            if (cartFromDb == null)
            {
                return NotFound(new { success = false, message = "Item cannot does not exist from cart!" });
            }

            _unitOfWork.Cart.Remove(cartFromDb);
            
            _unitOfWork.Save();
            
            return Ok(new { success = true, message = "Item deleted from cart!" });
        }

        private async Task<string> GetUserIdFromHeader()
        {
            var request = _contextAccessor.HttpContext?.Request;

            if (request is null ||
                !request.Headers.TryGetValue("Authorization", out var authorizationToken))
            {
                return default!;
            }

            return await _commandDataClient.GetId(authorizationToken.ToString());
        }
    }
}
