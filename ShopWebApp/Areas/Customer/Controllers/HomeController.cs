using Microsoft.AspNetCore.Mvc;

namespace ShopWebApp.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
