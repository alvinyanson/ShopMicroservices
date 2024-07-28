using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWebApp.Services.Contracts;
using ShopWebApp.Services.HttpClients;
using ShopWebApp.Services;
using ShopWebApp.Models;
using System.Text.Json;
using ShopWebApp.Dtos;

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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangeUsernameAndEmail()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(Login request)
        {
            HttpResponseMessage? response = await _httpService.PostAsync(HttpContext, request, "Login");

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

                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            else
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

                ModelState.AddModelError("", apiResponse.Message);
                return View(nameof(Login));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register request)
        {
            var credentialsDto = _mapper.Map<RegisterDto>(request);
            HttpResponseMessage? response = await _httpService.PostAsync(HttpContext, credentialsDto, "Register");

            if (response == null)
            {
                ModelState.AddModelError("", "Unable to connect to authentication service. Please contact admin.");
                return View();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            if(!apiResponse.Success)
            {
                ModelState.AddModelError("", apiResponse.Message);

                return View(nameof(Register));
            }

            return await Login(_mapper.Map<Login>(request));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUsernameAndEmail(ChangeUsernameAndEmailDto request)
        {
            HttpResponseMessage? response = await _httpService.PostAsync(HttpContext, request, "ChangeEmailAndUsername");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request)
        {
            HttpResponseMessage? response = await _httpService.PostAsync(HttpContext, request, "ChangePassword");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            if(!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", apiResponse.Message);
                return View(nameof(ChangePassword));
            }

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync(HttpContext);
            return RedirectToAction(nameof(Login));
        }
    }
}
