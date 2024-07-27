using Microsoft.AspNetCore.Mvc;

namespace ShopWebApp.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
