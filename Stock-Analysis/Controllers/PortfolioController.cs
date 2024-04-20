using BusinessLayer.DTO;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class PortfolioController : Controller
    {
        private IPortfolioService _portfolioService;
        public PortfolioController(IPortfolioService service)
        {
            _portfolioService = service;
        }
        public IActionResult Index()
        {
            return View();
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
    }
}
