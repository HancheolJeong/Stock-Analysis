using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Stock_Analysis.Controllers
{
    public class KOSPIController : Controller
    {
        private readonly IStockService _stockService;
        private const int _PageSize = 100;
        private const string _market = "KOSPI";
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


        [HttpGet("KOSPI/ohlcv")]
        public async Task<IActionResult> OHLCV(string ticker, string axisy)
        {
            var stocks = await _stockService.GetStockOHLCV(ticker);
            string name = _stockService.GetNameByTicker(_market ,ticker);
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market; 
            ViewBag.axisy = axisy;
            return View(stocks);
        }

        [HttpGet("KOSPI/fundamental")]
        public async Task<IActionResult> Fundamental(string ticker, string axisy)
        {
            var stocks = await _stockService.GetStockFundamental(ticker);
            string name = _stockService.GetNameByTicker(_market, ticker);
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market;
            ViewBag.axisy = axisy;
            return View(stocks);
        }

        [HttpGet("KOSPI/marketcap")]
        public async Task<IActionResult> MarketCap(string ticker, string axisy)
        {
            var stocks = await _stockService.GetStockMarketCap(ticker);
            string name = _stockService.GetNameByTicker(_market, ticker);
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market;
            ViewBag.axisy = axisy;
            return View(stocks);
        }

        [HttpGet("KOSPI/markettrx")]
        public async Task<IActionResult> MarketTRX(string ticker, string axisy)
        {
            var stocks = await _stockService.GetStockMarketTRX(ticker);
            string name = _stockService.GetNameByTicker(_market, ticker);
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market;
            ViewBag.axisy = axisy;
            return View(stocks);
        }

        [HttpGet("KOSPI/sectortrx")]
        public async Task<IActionResult> SectorTRX(string ticker, string axisy)
        {
            var stocks = await _stockService.GetStockSectorTRX(ticker);
            string name = _stockService.GetNameByTicker(_market, ticker);
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market;
            ViewBag.axisy = axisy;
            return View(stocks);
        }

    }
}
