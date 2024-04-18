using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class PortfolioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
