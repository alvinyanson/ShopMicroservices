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
        private readonly IHttpServiceWrapper _httpService;

        public HomeController(IConfiguration config, AuthService authService)
        {
            _httpService = new HttpService<HttpProductCatalogService>(config, authService);
        }


        public async Task<IActionResult> Index()
        {

            HttpResponseMessage? response = await _httpService.GetAsync(HttpContext, "Products");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<ProductDto>>>(jsonResponse);

            return View(apiResponse.Result);
        }
    }
}
