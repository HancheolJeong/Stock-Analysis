using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class KOSDAQController : Controller
    {
        private readonly IStockService _stockService;
        private const int _PageSize = 100;
        private const string _market = "KOSDAQ";
        public KOSDAQController(IStockService service)
        {
            _stockService = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET /kosdaq?pageNumber
        /// Stock 리스트를 페이지 번호와 KOSDAQ 기준으로 페이지 요청
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("KOSDAQ/{pageNumber:int}")]
        public IActionResult Index(int pageNumber = 1)
        {
            var stocks = _stockService.GetStock(_market, pageNumber, _PageSize);
            int totalPages = _stockService.GetStockCount(_market, _PageSize);
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

        /// <summary>
        /// GET /kosdaq/search?query&pageNumber
        /// 검색어로 stock 리스트를 받아오고 페이지번호를 기준으로 분리하고 페이지 요청
        /// </summary>
        /// <param name="query">검색어</param>
        /// <param name="pageNumber">페이지 번호</param>
        /// <returns></returns>
        [HttpGet("KOSDAQ/search")]
        public IActionResult Search(string query, int pageNumber = 1)
        {

            var stocks = _stockService.SearchStock(_market, query);
            int totalPages = _stockService.GetPageCountByDTO(ref stocks, _PageSize);
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
		/// GET /kosdaq/ohlcv?ticker&axisy
		/// 티커를 기준으로 Stock OHLCVㅌ 테이블을 불러오고 종목명, 티커, 시장, y좌표를 설정하고 페이지를 요청
		/// </summary>
		/// <param name="ticker">티커</param>
		/// <param name="axisy">라인차트 y축 좌표</param>
		/// <returns></returns>
		[HttpGet("KOSDAQ/ohlcv")]
        public async Task<IActionResult> OHLCV(string ticker, string axisy)
        {
            var stocks = await _stockService.GetStockOHLCV(ticker);
            string name = _stockService.GetNameByTicker(_market, ticker);
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market;
            ViewBag.axisy = axisy;
            return View(stocks);
        }

		/// <summary>
		/// GET /kosdaq/fundamental?ticker&axisy
		/// 티커를 기준으로 Stock Fundamental 테이블을 불러오고 종목명, 티커, 시장, y좌표를 설정하고 페이지를 요청
		/// </summary>
		/// <param name="ticker">티커</param>
		/// <param name="axisy">라인차트 y축 좌표</param>
		/// <returns></returns>
		[HttpGet("KOSDAQ/fundamental")]
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

		/// <summary>
		/// GET /kosdaq/marketcap?ticker&axisy
		/// 티커를 기준으로 Stock MarcketCap 테이블을 불러오고 종목명, 티커, 시장, y좌표를 설정하고 페이지를 요청
		/// </summary>
		/// <param name="ticker">티커</param>
		/// <param name="axisy">라인차트 y축 좌표</param>
		/// <returns></returns>
		[HttpGet("KOSDAQ/marketcap")]
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

		/// <summary>
		/// GET /kosdaq/markettrx?ticker&axisy
		/// 티커를 기준으로 Stock MarcketTRX 테이블을 불러오고 종목명, 티커, 시장, y좌표를 설정하고 페이지를 요청
		/// </summary>
		/// <param name="ticker">티커</param>
		/// <param name="axisy">라인차트 y축 좌표</param>
		/// <returns></returns>
		[HttpGet("KOSDAQ/markettrx")]
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

		/// <summary>
		/// GET /kosdaq/sectortrx?ticker&axisy
		/// 티커를 기준으로 Stock SectorTRX 테이블을 불러오고 종목명, 티커, 시장, y좌표를 설정하고 페이지를 요청
		/// </summary>
		/// <param name="ticker">티커</param>
		/// <param name="axisy">라인차트 y축 좌표</param>
		/// <returns></returns>
		[HttpGet("KOSDAQ/sectortrx")]
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
