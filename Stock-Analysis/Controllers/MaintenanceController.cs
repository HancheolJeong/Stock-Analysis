using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class MaintenanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
