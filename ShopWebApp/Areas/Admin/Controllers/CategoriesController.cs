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
    public class CategoriesController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IHttpServiceWrapper _productCatalogService;

        public CategoriesController(
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
            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, "Categories");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<CategoryDto>>>(jsonResponse);

            return View(apiResponse.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            if(id== null || id == 0)
            {
                return View(new Category());
            }

            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, $"Categories/{id}");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<CategoryDto>>(jsonResponse);

            return View(new Category()
            {
                Id = apiResponse.Result.Id,
                Name = apiResponse.Result.Name,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            HttpResponseMessage? response = await _productCatalogService.PostAsync(HttpContext, category, "Categories");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            TempData["success"] = apiResponse.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage? response = await _productCatalogService.DeleteAsync(HttpContext, $"Categories/{id}");
            
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            TempData["success"] = apiResponse.Message;

            return Json(new { success = true, message = apiResponse.Message });
        }
    }
}
