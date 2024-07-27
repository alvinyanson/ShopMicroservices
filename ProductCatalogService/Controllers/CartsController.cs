using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Dtos;
using ProductCatalogService.Models;
using ProductCatalogService.Services.Contracts;
using ProductCatalogService.SyncDataServices.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IHttpContextHelper _httpContextHelper;
        private readonly IHttpComms _httpComms;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartsController(
            IHttpContextHelper httpContextHelper,
            IHttpComms httpComms,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _httpContextHelper = httpContextHelper;
            _httpComms = httpComms;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetCartItems()
        {
            try
            {
                // Check if token is valid
                string token = _httpContextHelper.GetTokenFromHeaders();
                bool isValidToken = await ValidateTokenFromAuthService(token);

                if (string.IsNullOrEmpty(token) || !isValidToken)
                {
                    return Unauthorized();
                }

                // Retrieve cart items based on ownerId
                string ownerId = await GetUserIdFromAuthService(token);

                var cartItems = _unitOfWork.Cart.GetAll().Where(u => u.OwnerId == ownerId);

                return Ok(new { success = true, message = "Cart items retrieved!", result = cartItems });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = $"Something went wrong!" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddItemToCart(AddToCartDto addToCartDto)
        {
            try
            {
                // Check if token is valid
                string token = _httpContextHelper.GetTokenFromHeaders();
                bool isValidToken = await ValidateTokenFromAuthService(token);

                if (string.IsNullOrEmpty(token) || !isValidToken)
                {
                    return Unauthorized();
                }

                // Retrieve cart items based on ownerId
                string ownerId = await GetUserIdFromAuthService(token);

                Console.WriteLine($"OWNER ID 🤓: {ownerId}");

                Cart cartFromDb = _unitOfWork.Cart.Get(u => u.OwnerId == ownerId && u.ProductId == addToCartDto.ProductId);

                // Update item qty from cart if it's already existing product
                if (cartFromDb != null)
                {
                    cartFromDb.Quantity += addToCartDto.Quantity;

                    _unitOfWork.Cart.Update(cartFromDb);

                    _unitOfWork.Save();

                    return Ok(new { success = true, message = "Item updated from cart!" });
                }

                // Add item to cart
                else
                {
                    addToCartDto.OwnerId = ownerId;

                    var result = _mapper.Map<Cart>(addToCartDto);

                    _unitOfWork.Cart.Add(result);

                    _unitOfWork.Save();

                    return Ok(new { success = true, message = "Item added to cart!" });
                }

            }
            catch
            {
                return StatusCode(500, new { success = false, message = $"Something went wrong!" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteItemFrom(int id)
        {
            try
            {
                // Check if token is valid
                string token = _httpContextHelper.GetTokenFromHeaders();
                bool isValidToken = await ValidateTokenFromAuthService(token);

                if (string.IsNullOrEmpty(token) || !isValidToken)
                {
                    return Unauthorized();
                }

                // Retrieve cart items based on ownerId
                string ownerId = await GetUserIdFromAuthService(token);

                var cartFromDb = _unitOfWork.Cart.Get(u => u.OwnerId == ownerId && u.Id == id);

                if (cartFromDb == null)
                {
                    return NotFound(new { success = false, message = "Item does not exist from cart!" });
                }

                // Remove item from cart
                _unitOfWork.Cart.Remove(cartFromDb);

                _unitOfWork.Save();

                return Ok(new { success = true, message = "Item deleted from cart!" });
            }
            catch
            {
                return StatusCode(500, new { success = false, message = $"Something went wrong!" });
            }
        }

        [HttpGet(nameof(TestConnection))]
        public async Task<ActionResult<string>> TestConnection()
        {
            try
            {
                var message = await _httpComms.TestConnection();

                return Ok(message);
            }
            catch
            {
                return StatusCode(500, new { success = false, message = $"Something went wrong!" });
            }
        }

        private async Task<string> GetUserIdFromAuthService(string token)
        {
            try
            {
                return await _httpComms.GetUserId(token);
            }
            catch
            {
                return string.Empty;
            }
        }

        private async Task<bool> ValidateTokenFromAuthService(string token)
        {
            try
            {
                return await _httpComms.IsTokenValid(token);
            }
            catch
            {
                return false;
            }
        }
    }
}
