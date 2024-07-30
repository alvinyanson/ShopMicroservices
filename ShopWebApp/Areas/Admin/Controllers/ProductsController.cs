using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopWebApp.Services.Contracts;
using ShopWebApp.Services.HttpClients;
using ShopWebApp.Services;
using ShopWebApp.Dtos;
using ShopWebApp.Models;
using System.Text.Json;
using ShopWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShopWebApp.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpServiceWrapper _productCatalogService;
        private readonly IMapper _mapper;

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
        public async Task<IActionResult> Create(int? id)
        {
            // Get product categories
            HttpResponseMessage? response = await _productCatalogService.GetAsync(HttpContext, "Categories");

            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<ReadCategoryDto>>>(jsonResponse);

                if (parsedResponse != null)
                {
                    // Create Form Product
                    if (id == null || id == 0)
                    {
                        return View(new ProductVM()
                        {
                            Product = new Product(),
                            Categories = parsedResponse.Result.Select(u => new SelectListItem
                            {
                                Text = u.Name,
                                Value = u.Id.ToString()
                            })
                        });
                    }

                    // Edit Form Product, retrieve product info and seed the form
                    HttpResponseMessage? response2 = await _productCatalogService.GetAsync(HttpContext, $"Products/{id}");

                    if (response2 != null && response2.IsSuccessStatusCode)
                    {
                        var jsonResponse2 = await response2.Content.ReadAsStringAsync();

                        var parsedResponse2 = JsonSerializer.Deserialize<ApiResponse<ReadProductDto>>(jsonResponse2);

                        if (parsedResponse2 != null)
                        {
                            return View(new ProductVM()
                            {
                                Product = new Product()
                                {
                                    Id = parsedResponse2.Result.Id,
                                    Name = parsedResponse2.Result.Name,
                                    Description = parsedResponse2.Result.Description,
                                    ImageUrl = parsedResponse2.Result.ImageUrl,
                                    Price = parsedResponse2.Result.Price,
                                    CategoryId = parsedResponse2.Result.CategoryId
                                },
                                Categories = parsedResponse.Result.Select(u => new SelectListItem
                                {
                                    Text = u.Name,
                                    Value = u.Id.ToString()
                                })
                            });
                        }
                    }
                }


            }

            return View(new ProductVM());
        }


        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            HttpResponseMessage? response = await _productCatalogService.PostAsync(HttpContext, product, "Products");

            if (response != null && response.IsSuccessStatusCode)
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
            HttpResponseMessage? response = await _productCatalogService.DeleteAsync(HttpContext, $"Products/{id}");

            if (response != null)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

                if (parsedResponse != null)
                {
                    TempData["success"] = parsedResponse.Message;

                    return Json(new { success = true, message = parsedResponse.Message });
                }
            }

            return View();
        }
    }
}
