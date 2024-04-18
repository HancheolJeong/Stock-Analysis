using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class INDEXController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
