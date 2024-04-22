using BusinessLayer.DTO;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class PortfolioController : Controller
    {
        private IPortfolioService _portfolioService;
        private IStockService _stockService;
        private IETFService _etfService;
        public PortfolioController(IPortfolioService portfolioService, IStockService stockService, IETFService etfService)
        {
            _portfolioService = portfolioService;
            _stockService = stockService;
            _etfService = etfService;
        }

        public async Task<IActionResult> Index()
        {
            GetUserDTO? userDto = HttpContext.Session.Get<GetUserDTO>("LoginUser");
            
            if (userDto == null)
            {
                return RedirectToAction("/"); 
            }


            string email = userDto.email;
            var stocks = await _portfolioService.GetPortfolio(email);

            foreach (var stock in stocks)
            {
                if (stock.market == "KOSPI")
                {
                    stock.name = _stockService.GetNameByTicker("KOSPI",stock.ticker);
                    stock.current_price = _stockService.GetPriceByTicker("KOSPI", stock.ticker);
                }
                else if (stock.market == "KOSDAQ")
                {
                    stock.name = _stockService.GetNameByTicker("KOSDAQ", stock.ticker);
                    stock.current_price = _stockService.GetPriceByTicker("KOSDAQ", stock.ticker);
                }
                else if (stock.market == "ETF")
                {
                    stock.name = _etfService.GetNameByTicker(stock.ticker);
                    stock.current_price = _etfService.GetPriceByTicker(stock.ticker);
                }
            }

            int totalPurchaseCost = _portfolioService.GetTotalPurchaseCost(ref stocks);
            int totalValuationProfitLoss = _portfolioService.GetTotalValuationProfitLoss(ref stocks);
            int totalValuation = _portfolioService.GetTotalValuation(ref stocks);
            double totalReturnPercentage = _portfolioService.GetTotalReturnPercentage(ref stocks);
            ViewBag.totalPurchaseCost  = totalPurchaseCost;
            ViewBag.totalValuationProfitLoss = totalValuationProfitLoss;
            ViewBag.totalValuation = totalValuation;
            ViewBag.totalReturnPercentage = totalReturnPercentage;
            return View(stocks);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int quantity, int price, string ticker, string market)
        {
            GetUserDTO? userDto = HttpContext.Session.Get<GetUserDTO>("LoginUser");

            if (userDto == null)
            {
                return Json(new { success = false, message = "No user session found." });
            }

            CreatePortfolioDTO dto = new CreatePortfolioDTO
            {
                amount = quantity,
                unit_price = price,
                ticker = ticker,
                market = market,
                email = userDto.email
            };
            bool isSuccess = await _portfolioService.CreatePortfolio(dto);
            if(isSuccess)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			GetUserDTO? userDto = HttpContext.Session.Get<GetUserDTO>("LoginUser");
			if (userDto == null)
			{
				return Json(new { success = false, message = "User is not authenticated." });
			}

			bool success = await _portfolioService.DeletePortfolio(id);
			if (success)
			{
				return Json(new { success = true, message = "삭제 완료했습니다." });
			}
			else
			{
				return Json(new { success = false, message = "삭제 실패했습니다." });
			}
		}



	}
}
