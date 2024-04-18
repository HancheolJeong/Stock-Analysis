using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class PortfolioClass : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
