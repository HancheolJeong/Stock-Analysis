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


        public IActionResult Test()
        {
            GetUserResponseDTO? user = HttpContext.Session.Get<GetUserResponseDTO>("LoginUser");
            ViewData["MyMsg"] = "Hello response";
            ViewBag.MyTest = new List<string> {"abc", "kim", "lee" };
            ViewBag.MyNum = 5;
            var list = new List<String> { "abc", "kim", "lee"};
            if(user != null && user.Username != null)
            {
                list.Add(user.Username);
            }
            return View(list);
        }
    }
}
