using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopWebApp.Services.Contracts;
using ShopWebApp.Services.HttpClients;
using ShopWebApp.Services;
using ShopWebApp.Dtos;
using ShopWebApp.Models;
using System.Text.Json;

namespace ShopWebApp.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IHttpServiceWrapper _productCatalogService;

        public ProductsController(
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
    }
}
