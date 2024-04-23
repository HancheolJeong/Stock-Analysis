using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class ETFController : Controller
    {
        private readonly IETFService _etfService;
        private const int _PageSize = 100;
        private const string _market = "ETF";
        public ETFController(IETFService service)
        {
            _etfService = service;
        }
        /// <summary>
        /// GET /etf?pageNumber
        /// ETF 리스트를 페이지번호를 기준으로 페이지 요청
        /// </summary>
        /// <param name="pageNumber">페이지 번호</param>
        /// <returns></returns>
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


        /// <summary>
        /// GET /etf/search?query&pageNumber
        /// ETF 리스트를 받아오고 페이지를 기준으로 분리해서 페이지 요청
        /// </summary>
        /// <param name="query">검색어</param>
        /// <param name="pageNumber">페이지번호</param>
        /// <returns></returns>
        [HttpGet("ETF/search")]
        public IActionResult Search(string query, int pageNumber = 1)
        {
            var stocks = _etfService.SearchETF(query);
            int totalPages = _etfService.GetCountByDTO(ref stocks, _PageSize);
            var filteredstocks = stocks.Skip(pageNumber - 1).Take(+_PageSize).ToList();
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.query = query;
            if (pageNumber == 1) // 무한루프로 빠지는 것을 막기 위한 로직
            {

                return View(filteredstocks);
            }
            if (pageNumber < 1 || pageNumber > totalPages) // 페이자가 범위를 벗어나면 첫번째 페이지로 Redirect
            {
                return RedirectToAction("/search", new { query = query, pageNumber = 1 });
            }
            return View(filteredstocks);
        }


        /// <summary>
        /// GET /etf/ohlcv?ticker&axisy
        /// ETF OHLCV 리스트를 받아오고 종목명 티커 시장 y축 값을 설정하고 페이지 요청
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <param name="axisy">라인차트 y축 값</param>
        /// <returns></returns>
        [HttpGet("ETF/ohlcv")]
        public async Task<IActionResult> OHLCV(string ticker, string axisy)
        {
            var stocks = await _etfService.GetETFOHLCVDTO(ticker);
            string name = _etfService.GetNameByTicker(ticker);
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market;
            ViewBag.axisy = axisy;
            return View(stocks);
        }
    }
}
