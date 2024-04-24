using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
	public class ExceptionController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
