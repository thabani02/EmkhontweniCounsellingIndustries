using Microsoft.AspNetCore.Mvc;

namespace EmkhontweniRoyalCounselling.Controllers
{
    // Handles static pages: Home, About, Services, Pricing
    public class HomeController : Controller
    {
        // GET: /
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/About
        public IActionResult About()
        {
            return View();
        }

        // GET: /Home/Services
        public IActionResult Services()
        {
            return View();
        }

        // GET: /Home/Pricing
        public IActionResult Pricing()
        {
            return View();
        }
    }
}
