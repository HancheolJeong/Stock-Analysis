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

        /// <summary>
        /// GET /portfolio
        /// 포트폴리오를 요청 
        /// </summary>
        /// <returns></returns>
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
                if (stock.market == "KOSPI") // 시장 코스피일때
                {
                    stock.name = _stockService.GetNameByTicker("KOSPI",stock.ticker); 
                    stock.current_price = _stockService.GetPriceByTicker("KOSPI", stock.ticker);
                }
                else if (stock.market == "KOSDAQ") // 시장 코스닥일때
                {
                    stock.name = _stockService.GetNameByTicker("KOSDAQ", stock.ticker);
                    stock.current_price = _stockService.GetPriceByTicker("KOSDAQ", stock.ticker);
                }
                else if (stock.market == "ETF") // 시장 ETF 일때
                {
                    stock.name = _etfService.GetNameByTicker(stock.ticker);
                    stock.current_price = _etfService.GetPriceByTicker(stock.ticker);
                }
            }

            int totalPurchaseCost = _portfolioService.GetTotalPurchaseCost(ref stocks); // 총매수금액
            int totalValuationProfitLossCost = _portfolioService.GetTotalValuationProfitLossCost(ref stocks); //총평가손익
            int totalValuationCost = _portfolioService.GetTotalValuationCost(ref stocks); // 총평가금액
            double totalProfitLossPercentage = _portfolioService.GetTotalProfitLossPercentage(ref stocks); // 총평가손익률
            ViewBag.totalPurchaseCost  = totalPurchaseCost;
            ViewBag.totalValuationProfitLoss = totalValuationProfitLossCost;
            ViewBag.totalValuation = totalValuationCost;
            ViewBag.totalReturnPercentage = totalProfitLossPercentage;
            return View(stocks);
        }

        /// <summary>
        /// POST /portfolio/add?quantity&price&ticker&market
        /// 새로운 포트폴리오를 추가 수량, 단가, 티커, 시장으로 dto를 만들고 서비스에 생성을 요청한다.
        /// </summary>
        /// <param name="quantity">수량</param>
        /// <param name="price">단가</param>
        /// <param name="ticker">티커</param>
        /// <param name="market">시장</param>
        /// <returns></returns>
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

        /// <summary>
        /// DELETE /portfolio/delete?id
        /// 포트폴리오 삭제를 요청
        /// </summary>
        /// <param name="id">고유 ID</param>
        /// <returns></returns>
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
