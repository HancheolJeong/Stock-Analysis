using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class KOSDAQController : Controller
    {
        private readonly IStockService _stockService;
        private const int _PageSize = 100;
        public KOSDAQController(IStockService service)
        {
            _stockService = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("KOSDAQ/{pageNumber:int}")]
        public IActionResult Index(int pageNumber = 1)
        {
            var stocks = _stockService.GetKOSDAQ(pageNumber, _PageSize);
            int totalPages = _stockService.GetKOSDAQCount(_PageSize);
            if (pageNumber == 1) // 무한루프로 빠지는 것을 막기 위한 로직
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

        [HttpGet("KOSDAQ/search")]
        public IActionResult Search(string query)
        {
            var stocks = _stockService.SearchKOSDAQ(query);
            ViewBag.Query = query;
            return View(stocks);
        }

        [HttpGet("KOSDAQ/Detail")]
        public IActionResult Detail(string ticker)
        {
            // 여기서 ticker에 해당하는 종목의 상세 정보를 가져오는 로직을 추가하시면 됩니다.
            //var stockDetail = _stockService.GetStockDetail(ticker); // 예시로 _stockService에서 GetStockDetail 메서드를 호출하였습니다.

            // 가져온 상세 정보를 view에 전달합니다.
            //return View(stockDetail);
            return View();
        }
    }
}
