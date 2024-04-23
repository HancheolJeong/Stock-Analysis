using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
	public class HomeController : Controller
    {
        private readonly IIndexService _indexService;
        private readonly IStockService _stockService;

        public HomeController(IIndexService indexService, IStockService stockService)
        {
            _indexService = indexService;
            _stockService = stockService;
        }
        
        /// <summary>
        /// GET /
        /// 웹 페이지의 홈 코스피 코스닥 증시 종가 시가총액 Top5 거래대금 Top5를 불러옵니다.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {

            decimal kospiIndex = _indexService.SearchIndexByTicker("1001"); //코스피 INDEX
            decimal kosdaqIndex = _indexService.SearchIndexByTicker("2001"); //코스닥 INDEX
            var kospiMarketCapTOP5 = _stockService.GetTopStocksByValue("KOSPI", "marketCap", 5); //코스피 시가총액 TOP5
            var kosdaqMarketCapTOP5 = _stockService.GetTopStocksByValue("KOSDAQ", "marketCap", 5); //코스닥 시가총액 TOP5
            var kospiTransactionAmountTOP5 = _stockService.GetTopStocksByValue("KOSPI", "transactionAmount", 5); //코스피 거래대금 TOP5
            var kosdaqTransactionAmountTOP5 = _stockService.GetTopStocksByValue("KOSDAQ", "transactionAmount", 5); //코스닥 거래대금 TOP5

            ViewBag.KospiIndex = kospiIndex; //코스피 INDEX
			ViewBag.KosdaqIndex = kosdaqIndex; //코스닥 INDEX
			ViewBag.KospiMarketCapTOP5 = kospiMarketCapTOP5; //코스피 시가총액 TOP5
			ViewBag.KosdaqMarketCapTOP5 = kosdaqMarketCapTOP5; //코스닥 시가총액 TOP5
			ViewBag.KospiTransactionAmountTOP5 = kospiTransactionAmountTOP5; //코스피 거래대금 TOP5
			ViewBag.KosdaqTransactionAmountTOP5 = kosdaqTransactionAmountTOP5;  //코스닥 거래대금 TOP5

			return View();
        }

		public IActionResult Maintenance()
		{
			return View();
		}

	}
}
