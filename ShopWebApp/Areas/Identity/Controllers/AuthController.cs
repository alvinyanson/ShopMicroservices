using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWebApp.Services.Contracts;
using ShopWebApp.Services.HttpClients;
using ShopWebApp.Services;
using ShopWebApp.Models.ViewModels;
using ShopWebApp.Models;
using System.Text.Json;

namespace ShopWebApp.Areas.Identity.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IHttpServiceWrapper _httpService;
        private readonly AuthService _authService;

        public AuthController(
            IMapper mapper,
            IConfiguration config,
            AuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
            _httpService = new HttpService<HttpAuthService>(config, authService);
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            HttpResponseMessage? response = await _httpService.PostAsync(HttpContext, loginVM.Credential, "Login");

            if (response is null)
            {
                ModelState.AddModelError("", "Unable to connect to authentication service. Please contact admin.");
                return View();
            }

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

                await _authService.SignInAsync(HttpContext, apiResponse.Result);

                return RedirectToAction("Index", "Home", new { area = "Customer"});
            }
            else
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

                ModelState.AddModelError("", apiResponse.Message);
                return View();
            }
        }
    }
}
