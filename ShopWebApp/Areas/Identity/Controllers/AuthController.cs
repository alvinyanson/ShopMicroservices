using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWebApp.Services.Contracts;
using ShopWebApp.Services.HttpClients;
using ShopWebApp.Services;
using ShopWebApp.Models;
using System.Text.Json;
using ShopWebApp.Dtos;
using ShopWebApp.Models.ViewModels;

namespace ShopWebApp.Areas.Identity.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {

        private readonly IHttpServiceWrapper _httpService;
        private readonly AuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(
            AuthService authService,
            IConfiguration config,
            IMapper mapper)
        {
            _httpService = new HttpService<HttpAuthService>(config, authService);
            _authService = authService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                await _authService.RefreshTokenAsync(HttpContext);
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                await _authService.RefreshTokenAsync(HttpContext);
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            return View(new RegisterVM());
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

            if (response == null)
            {
                ModelState.AddModelError("", "Unable to connect to auth service.");
                return View();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var parsedResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            if (!response.IsSuccessStatusCode)
            {
                if (parsedResponse != null)
                {
                    AddErrorMessage(parsedResponse.Message);
                }

                return View(nameof(Login));
            }

            await _authService.SignInAsync(HttpContext, parsedResponse.Result);

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM request)
        {
            var credentialsDto = _mapper.Map<RegisterUserDto>(request.Register);
            HttpResponseMessage? response = await _httpService.PostAsync(HttpContext, credentialsDto, "Register");

            if (response == null)
            {
                ModelState.AddModelError("", "Unable to connect to authentication service.");
                return View();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var parsedResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            if (!response.IsSuccessStatusCode)
            {
                if (parsedResponse != null)
                {
                    AddErrorMessage(parsedResponse.Message);
                }

                return View(nameof(Register));
            }

            return await Login(_mapper.Map<Login>(request.Register));

        }

        [HttpPost]
        public async Task<IActionResult> ChangeUsernameAndEmail(ChangeUsernameAndEmailDto request)
        {
            HttpResponseMessage? response = await _httpService.PostAsync(HttpContext, request, "ChangeEmailAndUsername");

            if (response == null)
            {
                ModelState.AddModelError("", "Unable to connect to auth service.");
                return View();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var parsedResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            if (!response.IsSuccessStatusCode)
            {
                if (parsedResponse != null)
                {
                    AddErrorMessage(parsedResponse.Message);
                }

                return View(nameof(ChangeUsernameAndEmail));
            }

            TempData["success"] = parsedResponse.Message;

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request)
        {
            HttpResponseMessage? response = await _httpService.PostAsync(HttpContext, request, "ChangePassword");

            if (response == null)
            {
                ModelState.AddModelError("", "Unable to connect to auth service.");
                return View();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var parsedResponse = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);

            if (!response.IsSuccessStatusCode)
            {
                if (parsedResponse != null)
                {
                    AddErrorMessage(parsedResponse.Message);
                }

                return View(nameof(ChangePassword));
            }

            TempData["success"] = parsedResponse.Message;

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync(HttpContext);
            
            return RedirectToAction(nameof(Login));
        }

        private void AddErrorMessage(string errorMessage)
        {
            ModelState.AddModelError("", errorMessage);
        }
    }
}
