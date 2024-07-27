using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IHttpServiceWrapper _productCatalogService;

        public HomeController(
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

            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, "Products");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<ProductDto>>>(jsonResponse);

            return View(apiResponse.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, $"Products/{id}");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<ProductDto>>(jsonResponse);

            Cart cart = new()
            {
                Product = apiResponse.Result,
                Quantity = 1,
                ProductId = id
            };

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Details(Cart cart)
        {

            var item = _mapper.Map<AddItemToCartDto>(cart);

            HttpResponseMessage? response = await _productCatalogService.PostAsync(HttpContext, item, "Carts");
            
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            return RedirectToAction("Index");
        }
    }
}
