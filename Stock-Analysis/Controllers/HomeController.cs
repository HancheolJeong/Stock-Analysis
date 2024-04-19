using BusinessLayer.DTO;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public IActionResult Index()
        {

            decimal kospiIndex = _indexService.SearchIndexByTicker("1001"); //코스피 INDEX
            decimal kosdaqIndex = _indexService.SearchIndexByTicker("2001"); //코스닥 INDEX
            var kospiMarketCapTOP5 = _stockService.GetTopStocksByValue("KOSPI", "marketCap", 5); //코스피 시가총액 TOP5
            var kosdaqMarketCapTOP5 = _stockService.GetTopStocksByValue("KOSDAQ", "marketCap", 5); //코스닥 시가총액 TOP5
            var kospiTransactionAmountTOP5 = _stockService.GetTopStocksByValue("KOSPI", "transactionAmount", 5); //코스피 거래대금 TOP5
            var kosdaqTransactionAmountTOP5 = _stockService.GetTopStocksByValue("KOSDAQ", "transactionAmount", 5); //코스닥 거래대금 TOP5

            ViewBag.KospiIndex = kospiIndex;
            ViewBag.KosdaqIndex = kosdaqIndex;
            ViewBag.KospiMarketCapTOP5 = kospiMarketCapTOP5;
            ViewBag.KosdaqMarketCapTOP5 = kosdaqMarketCapTOP5;
            ViewBag.KospiTransactionAmountTOP5 = kospiTransactionAmountTOP5;
            ViewBag.KosdaqTransactionAmountTOP5 = kosdaqTransactionAmountTOP5;

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
    }
}
