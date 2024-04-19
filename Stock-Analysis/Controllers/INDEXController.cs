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
    }
}
