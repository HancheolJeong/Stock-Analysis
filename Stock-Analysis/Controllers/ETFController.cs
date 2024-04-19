using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class ETFController : Controller
    {
        private readonly IETFService _etfService;
        private const int _PageSize = 100;
        public ETFController(IETFService service)
        {
            _etfService = service;
        }
        [HttpGet("etf/{pageNumber:int}")]
        public IActionResult Index(int pageNumber = 1)
        {
            var indexes = _etfService.GetETF(pageNumber, _PageSize);
            int totalPages = _etfService.GetETFCount(_PageSize);
            if (pageNumber == 1) // 무한루프로 빠지는 것을 막기 위한 로직
            {
                ViewBag.CurrentPage = pageNumber;
                ViewBag.TotalPages = totalPages;
                return View(indexes);
            }
            if (pageNumber < 1 || pageNumber > totalPages) // 페이자가 범위를 벗어나면 첫번째 페이지로 Redirect
            {
                return RedirectToAction("ETF", new { pageNumber = 1 });
            }
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            return View(indexes);
        }
    }
}
