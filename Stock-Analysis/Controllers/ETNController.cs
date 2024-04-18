using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class ETNController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
