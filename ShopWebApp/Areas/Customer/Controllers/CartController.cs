using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopWebApp.Services.Contracts;
using ShopWebApp.Services.HttpClients;
using ShopWebApp.Services;
using ShopWebApp.Dtos;
using ShopWebApp.Models;
using System.Text.Json;
using ShopWebApp.Models.ViewModels;

namespace ShopWebApp.Areas.Customer.Controllers
{
    public class CartController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IHttpServiceWrapper _productCatalogService;

        public CartController(
            IConfiguration config,
            AuthService authService,
            IMapper mapper)
        {
            _mapper = mapper;
            _productCatalogService = new HttpService<HttpProductCatalogService>(config, authService);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, "Carts");

            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<ReadCartItemsDto>>>(jsonResponse);

                if (parsedResponse != null)
                {
                    var cartVM = new CartVM()
                    {
                        Items = parsedResponse.Result,
                    };

                    foreach (var cart in cartVM.Items)
                    {
                        cartVM.OrderTotal += (cart.Product.Price * cart.Quantity);
                    }
                 
                    return View(cartVM);
                }
            }

            return View(new CartVM());
        }

        public async Task<IActionResult> Plus(int id)
        {

            // Retrieve the item in cart
            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, $"Carts/{id}");

            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<ReadCartItemsDto>>(jsonResponse);

                if (parsedResponse != null)
                {
                    var item = new AddItemToCartDto()
                    {
                        ProductId = parsedResponse.Result.ProductId,
                        Quantity = parsedResponse.Result.Quantity += 1,
                    };

                    // Post the updated item to cart
                    await _productCatalogService.PostAsync(HttpContext, item, "Carts");
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Minus(int id)
        {
            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, $"Carts/{id}");

            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<ReadCartItemsDto>>(jsonResponse);

                if (parsedResponse != null)
                {
                    var item = new AddItemToCartDto()
                    {
                        ProductId = parsedResponse.Result.ProductId,
                        Quantity = parsedResponse.Result.Quantity -= 1,
                    };

                    await _productCatalogService.PostAsync(HttpContext, item, "Carts");
                }
            }
            else
            {
                TempData["error"] = "Something went wrong!";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            HttpResponseMessage? response = await _productCatalogService.DeleteAsync(HttpContext, $"Carts/{id}");

            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

                if (apiResponse != null)
                {
                    TempData["success"] = apiResponse.Message;
                }
            }
            else
            {
                TempData["error"] = "Something went wrong!";
            }

            return RedirectToAction("Index");
        }
    }
}
