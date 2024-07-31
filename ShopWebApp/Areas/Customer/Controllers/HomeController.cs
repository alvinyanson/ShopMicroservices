using Mapster;
using Microsoft.AspNetCore.Mvc;
using ShopWebApp.Dtos;
using ShopWebApp.Models;
using ShopWebApp.Services;
using ShopWebApp.Services.Contracts;
using ShopWebApp.Services.HttpClients;
using System.Text.Json;

namespace ShopWebApp.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpServiceWrapper _productCatalogService;

        public HomeController(
            AuthService authService,
            IConfiguration config)
        {
            _productCatalogService = new HttpService<HttpProductCatalogService>(config, authService);
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {

            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, "Products");

            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<ReadProductDto>>>(jsonResponse);

                if (parsedResponse != null)
                {
                    return View(parsedResponse.Result);
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, $"Products/{id}");

            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<ReadProductDto>>(jsonResponse);

                if (parsedResponse != null)
                {
                    Cart cart = new()
                    {
                        Product = parsedResponse.Result,
                        Quantity = 1,
                        ProductId = id
                    };

                    return View(cart);
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Details(Cart cart)
        {

            var item = cart.Adapt<AddItemToCartDto>();

            HttpResponseMessage? response = await _productCatalogService.PostAsync(HttpContext, item, "Carts");

            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

                if (parsedResponse != null)
                {
                    TempData["success"] = parsedResponse.Message;
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
