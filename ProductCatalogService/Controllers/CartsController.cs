using AutoMapper;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;

        public CartsController(
            IHttpContextHelper httpContextHelper,
            IHttpComms httpComms,
            IMapper mapper,
            ICartService cartService)
        {
            _httpContextHelper = httpContextHelper;
            _httpComms = httpComms;
            _mapper = mapper;
            _cartService = cartService;
        }

        [AuthHeaderFilter]
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

                var cartItems = _cartService.GetCartItemsByOwnerId(ownerId);

                return Ok(new { success = true, message = "Cart items retrieved!", result = cartItems });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = "Something went wrong!" });
            }
        }

        [AuthHeaderFilter]
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetCartItem(int id)
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

                // Retrieve ownerId
                string ownerId = await GetUserIdFromAuthService(token);
                
                // Retrieve cart item based on id and owner id
                var cartFromDb = _cartService.GetCartItemByIdAndOwnerId(id, ownerId);

                return Ok(new { success = true, message = "Cart item retrieved!", result = cartFromDb });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = "Something went wrong!" });
            }
        }

        [AuthHeaderFilter]
        [HttpPost]
        public async Task<ActionResult<string>> AddItemToCart(AddItemToCartDto addToCartDto)
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

                // HTTP Communication; Retrieve cart items based on ownerId
                string ownerId = await GetUserIdFromAuthService(token);

                Cart cart = _cartService.GetCartItemByProdIdAndOwnerId(addToCartDto.ProductId, ownerId);

                // Update item qty from cart if it's already existing product
                if (cart != null)
                {
                    cart.Quantity = addToCartDto.Quantity;

                    _cartService.UpdateCartItem(cart);

                    return Ok(new { success = true, message = "Item updated from cart!" });
                }

                // Add item to cart
                else
                {
                    addToCartDto.OwnerId = ownerId;

                    var result = _mapper.Map<Cart>(addToCartDto);

                    _cartService.AddCartItem(result);

                    return Ok(new { success = true, message = "Item added to cart!" });
                }
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "Something went wrong!" });
            }
        }

        [AuthHeaderFilter]
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> RemoveCartItem(int id)
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

                // Retrieve ownerId
                string ownerId = await GetUserIdFromAuthService(token);

                // Retrieve cart item based on id and owner id
                var cart = _cartService.GetCartItemByIdAndOwnerId(id, ownerId);

                if (cart == null)
                {
                    return NotFound(new { success = false, message = "Item does not exist from cart!" });
                }

                // Remove item from cart
                _cartService.RemoveCartItem(cart);

                return Ok(new { success = true, message = "Item deleted from cart!" });
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "Something went wrong!" });
            }
        }

        [AuthHeaderFilter]
        [HttpGet(nameof(Checkout))]
        public async Task<ActionResult<string>> Checkout()
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

                _cartService.Checkout(ownerId);

                return Ok(new { success = true, message = "Item checkout successfully!" });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = "Something went wrong!" });
            }
        }

        //HTTP CONNECTIONS FROM AUTHSERVICE

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
                return StatusCode(500, new { success = false, message = "Something went wrong!" });
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
