using Microsoft.AspNetCore.Mvc;

namespace ECommerce.AdminApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
