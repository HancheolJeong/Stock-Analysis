using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class ETFController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
