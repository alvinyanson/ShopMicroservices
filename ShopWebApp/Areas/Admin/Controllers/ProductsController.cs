using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopWebApp.Services.Contracts;
using ShopWebApp.Services.HttpClients;
using ShopWebApp.Services;
using ShopWebApp.Dtos;
using ShopWebApp.Models;
using System.Text.Json;
using ShopWebApp.Models.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {

            HttpResponseMessage? response2 = await _productCatalogService.GetAsync(HttpContext, "Categories");

            var jsonResponse2 = await response2.Content.ReadAsStringAsync();

            var apiResponse2 = JsonSerializer.Deserialize<ApiResponse<IEnumerable<CategoryDto>>>(jsonResponse2);

            if (id == null || id == 0)
            {
                return View(new ProductVM()
                { 
                    Product = new Product(),
                    Categories = apiResponse2.Result.Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    })
                });
            }

            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, $"Products/{id}");
            
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<ProductDto>>(jsonResponse);

            return View(new ProductVM()
            {
                Product = new Product()
                {
                    Id = apiResponse.Result.Id,
                    Name = apiResponse.Result.Name,
                    Description = apiResponse.Result.Description,
                    ImageUrl = apiResponse.Result.ImageUrl,
                    Price = apiResponse.Result.Price,
                },
                Categories = apiResponse2.Result.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            });
        }


        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            HttpResponseMessage? response = await _productCatalogService.PostAsync(HttpContext, product, "Products");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            TempData["success"] = apiResponse.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage? response = await _productCatalogService.DeleteAsync(HttpContext, $"Products/{id}");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            TempData["success"] = apiResponse.Message;

            return Json(new { success = true, message = apiResponse.Message });
        }
    }
}
