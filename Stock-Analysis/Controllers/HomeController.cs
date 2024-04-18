using BusinessLayer.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string userid, int age)
        {
            //if()
            return Redirect("/home/test");
        }

    }
}
