using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace Stock_Analysis.Controllers
{
    public class KOSPIController : Controller
    {
        private readonly IStockService _stockService;
        private const int _PageSize = 100;
        public KOSPIController(IStockService service)
        {
            _stockService = service;
        }
        [HttpGet("KOSPI/{pageNumber:int}")]
        public IActionResult Index(int pageNumber = 1)
        {
            var stocks = _stockService.GetKOSPI(pageNumber, _PageSize);
            int totalPages = _stockService.GetKOSPICount(_PageSize);
            if(pageNumber == 1) // 무한루프로 빠지는 것을 막기 위한 로직
            {
                ViewBag.CurrentPage = pageNumber;
                ViewBag.TotalPages = totalPages;
                return View(stocks);
            }
            if (pageNumber < 1 || pageNumber > totalPages) // 페이자가 범위를 벗어나면 첫번째 페이지로 Redirect
            {
                return RedirectToAction("Index", new { pageNumber = 1 });
            }
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            return View(stocks);
        }

        [HttpGet("KOSPI/search")]
        public IActionResult Search(string query)
        {
            var stocks = _stockService.SearchKOSPI(query);
            ViewBag.Query = query;
            return View(stocks);
        }
    }
}
