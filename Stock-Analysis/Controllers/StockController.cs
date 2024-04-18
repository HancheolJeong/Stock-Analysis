using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class StockController : Controller
    {
        private IStockService stockService;
        public StockController(IStockService service) 
        {
            stockService = service;
        }
        public async Task<IActionResult> Index()
        {
            return View(await stockService.GetStockInfo());
        }

        public IActionResult Test()
        {
            return View();
        }


    }
}
