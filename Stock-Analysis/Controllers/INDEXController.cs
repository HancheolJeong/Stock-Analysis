using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class INDEXController : Controller
    {
        private readonly IIndexService _indexService;
        private const int _PageSize = 100;
        public INDEXController(IIndexService service)
        {
            _indexService = service;
        }

        /// <summary>
        /// GET /index?pageNumber
        /// Index 리스트를 페이지 번호를 기준으로 페이지 요청
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("Index/{pageNumber:int}")]
        public IActionResult Index(int pageNumber = 1)
        {
            var indexes = _indexService.GetIndex(pageNumber, _PageSize);
            int totalPages = _indexService.GetIndexCount(_PageSize);
            if (pageNumber == 1) // 무한루프로 빠지는 것을 막기 위한 로직
            {
                ViewBag.CurrentPage = pageNumber;
                ViewBag.TotalPages = totalPages;
                return View(indexes);
            }
            if (pageNumber < 1 || pageNumber > totalPages) // 페이자가 범위를 벗어나면 첫번째 페이지로 Redirect
            {
                return RedirectToAction("Index", new { pageNumber = 1 });
            }
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            return View(indexes);
        }

        /// <summary>
        /// GET /index/search?query&pageNumber
        /// Index 리스트를 받아오고 페이지를 기준으로 분리해서 페이지 요청
        /// </summary>
        /// <param name="query">검색어</param>
        /// <param name="pageNumber">페이지 번호</param>
        /// <returns></returns>
        [HttpGet("Index/search")]
        public IActionResult Search(string query, int pageNumber = 1)
        {
            var stocks = _indexService.SearchIndex(query);
            int totalPages = _indexService.GetCountByDTO(ref stocks, _PageSize);
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
        /// GET /index/ohlcv?ticker&axisy
        /// Index OHLCV 리스트를 받아오고 종목명 티커 y축을 성정라고 페이지 요청
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <param name="axisy">라인차트 y축</param>
        /// <returns></returns>
        [HttpGet("Index/ohlcv")]
        public async Task<IActionResult> OHLCV(string ticker, string axisy)
        {
            var stocks = await _indexService.GetIndexOHLCV(ticker);
            string name = _indexService.GetNameByTicker(ticker);
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.axisy = axisy;
            return View(stocks);
        }

    }
}
