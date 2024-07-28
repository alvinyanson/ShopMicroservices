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

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<CartDto>>>(jsonResponse);

            var cartVM = new CartVM()
            {
                Items = apiResponse.Result,
            };

            foreach (var cart in cartVM.Items)
            {
                cartVM.OrderTotal += (cart.Product.Price * cart.Quantity);
            }

            return View(cartVM);
        }

        public async Task<IActionResult> Plus(int id)
        {

            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, $"Carts/{id}");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<CartDto>>(jsonResponse);


            var item = new AddItemToCartDto()
            {
                ProductId = apiResponse.Result.ProductId,
                Quantity = apiResponse.Result.Quantity += 1,
            };

            HttpResponseMessage? response2 = await _productCatalogService.PostAsync(HttpContext, item, "Carts");

            var jsonResponse2 = await response2.Content.ReadAsStringAsync();

            var apiResponse2 = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse2);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Minus(int id)
        {

            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, $"Carts/{id}");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<CartDto>>(jsonResponse);


            var item = new AddItemToCartDto()
            {
                ProductId = apiResponse.Result.ProductId,
                Quantity = apiResponse.Result.Quantity -= 1,
            };

            HttpResponseMessage? response2 = await _productCatalogService.PostAsync(HttpContext, item, "Carts");

            var jsonResponse2 = await response2.Content.ReadAsStringAsync();

            var apiResponse2 = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse2);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            HttpResponseMessage? response = await _productCatalogService.DeleteAsync(HttpContext, $"Carts/{id}");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            TempData["success"] = apiResponse.Message;

            return RedirectToAction("Index");
        }
    }
}
