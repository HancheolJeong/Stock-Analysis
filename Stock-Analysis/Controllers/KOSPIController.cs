﻿using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
	public class KOSPIController : Controller
    {
        private readonly IStockService _stockService;
        private const int _PageSize = 100;
        private const string _market = "KOSPI";
        private readonly ILogger<KOSPIController> _logger;
        public KOSPIController(IStockService service, ILogger<KOSPIController> logger)
        {
            _stockService = service;
            _logger = logger;
        }

		/// <summary>
		/// GET /kospi?pageNumber
		/// Stock 리스트를 페이지 번호와 KOSPI 기준으로 페이지 요청
		/// </summary>
		/// <param name="pageNumber"></param>
		/// <returns></returns>
		[HttpGet("KOSPI/{pageNumber:int}")]
        public IActionResult Index(int pageNumber = 1)
        {
            (var stocks, string? err) = _stockService.GetStock(_market, pageNumber, _PageSize);
            (int totalPages, string? err2) = _stockService.GetStockCount(_market, _PageSize);
            if (err != null || err2 != null)
            {
                if (err != null)
                {
                    _logger.LogError(err);
                }
                else
                {
                    _logger.LogError(err2);
                }
                return Redirect("/Exception");
            }
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            if (pageNumber == 1) // 무한루프로 빠지는 것을 막기 위한 로직
            {

                return View(stocks);
            }
            if (pageNumber < 1 || pageNumber > totalPages) // 페이자가 범위를 벗어나면 첫번째 페이지로 Redirect
            {
                return RedirectToAction("Index", new { pageNumber = 1 });
            }
            return View(stocks);
        }

		/// <summary>
		/// GET /kospi/search?query&pageNumber
		/// 검색어로 stock 리스트를 받아오고 페이지번호를 기준으로 분리하고 페이지 요청
		/// </summary>
		/// <param name="query">검색어</param>
		/// <param name="pageNumber">페이지 번호</param>
		/// <returns></returns>
		[HttpGet("KOSPI/search")]
        public IActionResult Search(string query, int pageNumber = 1)
        {
            (var stocks, string? err) = _stockService.SearchStock(_market, query);
            (int totalPages, string? err2) = _stockService.GetPageCountByDTO(ref stocks, _PageSize);
            if (err != null || err2 != null)
            {
                if (err != null)
                {
                    _logger.LogError(err);
                }
                else
                {
                    _logger.LogError(err2);
                }
                return Redirect("/Exception");
            }
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
                //return RedirectToAction("/search", new { query = query, pageNumber = 1 });
                return RedirectToAction("Index", "KOSPI", new { query = query, pageNumber = 1 });
            }
            return View(filteredstocks);
        }

		/// <summary>
		/// GET /kospi/ohlcv?ticker&axisy
		/// 티커를 기준으로 Stock OHLCV 테이블을 불러오고 종목명, 티커, 시장, y좌표를 설정하고 페이지를 요청
		/// </summary>
		/// <param name="ticker">티커</param>
		/// <param name="axisy">라인차트 y축 좌표</param>
		/// <returns></returns>
		[HttpGet("KOSPI/ohlcv")]
        public async Task<IActionResult> OHLCV(string ticker, string axisy)
        {
            (var stocks, string? err) = await _stockService.GetStockOHLCV(ticker);
            (string name, string? err2) = _stockService.GetNameByTicker(_market, ticker);
            if (err != null || err2 != null)
            {
                if (err != null)
                {
                    _logger.LogError(err);
                }
                else
                {
                    _logger.LogError(err2);
                }
                return Redirect("/Exception");
            }
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market; 
            ViewBag.axisy = axisy;
            return View(stocks);
        }

		/// <summary>
		/// GET /kospi/fundamental?ticker&axisy
		/// 티커를 기준으로 Stock Fundamental 테이블을 불러오고 종목명, 티커, 시장, y좌표를 설정하고 페이지를 요청
		/// </summary>
		/// <param name="ticker">티커</param>
		/// <param name="axisy">라인차트 y축 좌표</param>
		/// <returns></returns>
		[HttpGet("KOSPI/fundamental")]
        public async Task<IActionResult> Fundamental(string ticker, string axisy)
        {
            (var stocks, string? err) = await _stockService.GetStockFundamental(ticker);
            (string name, string? err2) = _stockService.GetNameByTicker(_market, ticker);
            if (err != null || err2 != null)
            {
                if (err != null)
                {
                    _logger.LogError(err);
                }
                else
                {
                    _logger.LogError(err2);
                }
                return Redirect("/Exception");
            }
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market;
            ViewBag.axisy = axisy;
            return View(stocks);
        }

		/// <summary>
		/// GET /kospi/marketcap?ticker&axisy
		/// 티커를 기준으로 Stock MarcketCap 테이블을 불러오고 종목명, 티커, 시장, y좌표를 설정하고 페이지를 요청
		/// </summary>
		/// <param name="ticker">티커</param>
		/// <param name="axisy">라인차트 y축 좌표</param>
		/// <returns></returns>
		[HttpGet("KOSPI/marketcap")]
        public async Task<IActionResult> MarketCap(string ticker, string axisy)
        {
            (var stocks, string? err) = await _stockService.GetStockMarketCap(ticker);
            (string name, string? err2) = _stockService.GetNameByTicker(_market, ticker);
            if (err != null || err2 != null)
            {
                if (err != null)
                {
                    _logger.LogError(err);
                }
                else
                {
                    _logger.LogError(err2);
                }
                return Redirect("/Exception");
            }
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market;
            ViewBag.axisy = axisy;
            return View(stocks);
        }

		/// <summary>
		/// GET /kospi/markettrx?ticker&axisy
		/// 티커를 기준으로 Stock MarcketTRX 테이블을 불러오고 종목명, 티커, 시장, y좌표를 설정하고 페이지를 요청
		/// </summary>
		/// <param name="ticker">티커</param>
		/// <param name="axisy">라인차트 y축 좌표</param>
		/// <returns></returns>
		[HttpGet("KOSPI/markettrx")]
        public async Task<IActionResult> MarketTRX(string ticker, string axisy)
        {
            (var stocks, string? err) = await _stockService.GetStockMarketTRX(ticker);
            (string name, string? err2) = _stockService.GetNameByTicker(_market, ticker);
            if (err != null || err2 != null)
            {
                if (err != null)
                {
                    _logger.LogError(err);
                }
                else
                {
                    _logger.LogError(err2);
                }
                return Redirect("/Exception");
            }
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market;
            ViewBag.axisy = axisy;
            return View(stocks);
        }

		/// <summary>
		/// GET /kospi/sectortrx?ticker&axisy
		/// 티커를 기준으로 Stock SectorTRX 테이블을 불러오고 종목명, 티커, 시장, y좌표를 설정하고 페이지를 요청
		/// </summary>
		/// <param name="ticker">티커</param>
		/// <param name="axisy">라인차트 y축 좌표</param>
		/// <returns></returns>
		[HttpGet("KOSPI/sectortrx")]
        public async Task<IActionResult> SectorTRX(string ticker, string axisy)
        {
            (var stocks, string? err) = await _stockService.GetStockSectorTRX(ticker);
            (string name, string? err2) = _stockService.GetNameByTicker(_market, ticker);
            if (err != null || err2 != null)
            {
                if (err != null)
                {
                    _logger.LogError(err);
                }
                else
                {
                    _logger.LogError(err2);
                }
                return Redirect("/Exception");
            }
            ViewBag.name = name;
            ViewBag.ticker = ticker;
            ViewBag.market = _market;
            ViewBag.axisy = axisy;
            return View(stocks);
        }

    }
}
