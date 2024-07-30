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
        private readonly IHttpServiceWrapper _productCatalogService;
        private readonly IMapper _mapper;

        public CategoriesController(
            AuthService authService,
            IConfiguration config,
            IMapper mapper)
        {
            _mapper = mapper;
            _productCatalogService = new HttpService<HttpProductCatalogService>(config, authService);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, "Categories");

            if(response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<ReadCategoryDto>>>(jsonResponse);

                if(parsedResponse != null)
                {
                    return View(parsedResponse.Result);
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            // Create category
            if (id == null || id == 0)
            {
                return View(new Category());
            }


            // Edit Category, retrieve category detail
            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, $"Categories/{id}");

            if(response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<ReadCategoryDto>>(jsonResponse);

                if(parsedResponse != null)
                {
                    return View(new Category()
                    {
                        Id = parsedResponse.Result.Id,
                        Name = parsedResponse.Result.Name,
                    });
                }
            }

            return View(new Category());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            HttpResponseMessage? response = await _productCatalogService.PostAsync(HttpContext, category, "Categories");

            if(response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

                if (parsedResponse != null)
                {
                    TempData["success"] = parsedResponse.Message;

                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage? response = await _productCatalogService.DeleteAsync(HttpContext, $"Categories/{id}");

            if(response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

                if(parsedResponse != null)
                {
                    TempData["success"] = parsedResponse.Message;

                    return Json(new { success = true, message = parsedResponse.Message });
                }
            }

            return View();
        }
    }
}
