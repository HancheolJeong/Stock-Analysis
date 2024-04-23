using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class StockService : IStockService
    {
        IStockMapper stockMapper; // Stock테이블 데이터베이스 매핑 인터페이스 선언
        IMemoryCache _memoryCache; 
        public StockService(IMemoryCache memoryCache, IStockMapper mapper)
        {
            stockMapper = mapper;
            _memoryCache = memoryCache;
        }



        /// <summary>
        /// 자주 접근하는 Stock 데이터를 캐시메모리에 적재합니다.
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataAsync()
        {

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Stock, GetStockDTO>());
            Mapper mapper = new Mapper(configuration);

            List<Stock> list = await stockMapper.GetAdvancedStockData();
            List<GetStockDTO> dtoList = mapper.Map<List<Stock>, List<GetStockDTO>>(list);
            var kospiStocks = dtoList.Where(s => s.market == "KOSPI").ToList();
            var kosdaqStocks = dtoList.Where(s => s.market == "KOSDAQ").ToList();

            _memoryCache.Set("KOSPI", kospiStocks, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
            _memoryCache.Set("KOSDAQ", kosdaqStocks, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
        }

        public string GetNameByTicker(string market, string ticker)
        {
            if (_memoryCache.TryGetValue(market, out List<GetStockDTO> stocks))
            {
                var stock = stocks.FirstOrDefault(s => s.ticker == ticker);
                if (stock != null)
                {
                    return stock.name;
                }
            }
            return "Not found";
        }

        public int GetPriceByTicker(string market, string ticker)
        {
            if (_memoryCache.TryGetValue(market, out List<GetStockDTO> stocks))
            {
                var stock = stocks.FirstOrDefault(s => s.ticker == ticker);
                if (stock != null)
                {
                    return stock.closing_price;
                }
            }
            return 0;
        }



        public int GetCountByDTO(ref List<GetStockDTO> stocks, int pageSize)
        {
            if (stocks != null && stocks.Count > 0)
            {
                int result = (int)Math.Ceiling(stocks.Count / (double)pageSize);
                return result;
            }
            return 1;
        }

        public List<GetStockDTO> GetStock(string market ,int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue(market, out List<GetStockDTO> stocks))
            {
                return stocks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<GetStockDTO>();
        }



        public int GetKOSPICount(int pageSize)
        {
            if (_memoryCache.TryGetValue("KOSPI", out List<GetStockDTO> stocks))
            {
                int result = (int)Math.Ceiling(stocks.Count() / (double)pageSize);
                return result;
            }
            return 1;
        }

        public int GetKOSDAQCount(int pageSize)
        {
            if (_memoryCache.TryGetValue("KOSDAQ", out List<GetStockDTO> stocks))
            {
                return stocks.Count() / pageSize;
            }
            return 1;
        }

        public List<GetStockDTO> SearchStock(string market, string query)
        {
            if (_memoryCache.TryGetValue(market, out List<GetStockDTO> stocks))
            {
                return stocks.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList();
            }
            return new List<GetStockDTO>();
        }

        public List<GetStockDTO> GetTopStocksByValue(string marketKey, string sortBy, int n)
        {
            if (_memoryCache.TryGetValue(marketKey, out List<GetStockDTO> stocks))
            {
                switch (sortBy)
                {
                    case "marketCap":
                        return stocks.OrderByDescending(s => s.market_value).Take(n).ToList();
                    case "transactionAmount":
                        return stocks.OrderByDescending(s => s.transaction_amount).Take(n).ToList();
                    default:
                        throw new ArgumentException("Invalid sort parameter. Use 'market_value' or 'transaction_amount'.");
                }
            }
            return new List<GetStockDTO>();
        }


        public async Task<List<GetStockOHLCVDTO>> GetStockOHLCV(string ticker)
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<StockOHLCV, GetStockOHLCVDTO>());
                Mapper mapper = new Mapper(configuration);

                List<StockOHLCV> list = await stockMapper.GetStockOHLCV(ticker);


                List<GetStockOHLCVDTO> dtoList = mapper.Map<List<StockOHLCV>, List<GetStockOHLCVDTO>>(list);
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GetStockFundamentalDTO>> GetStockFundamental(string ticker)
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<StockFundamental, GetStockFundamentalDTO>());
                Mapper mapper = new Mapper(configuration);

                List<StockFundamental> list = await stockMapper.GetStockFundamental(ticker);


                List<GetStockFundamentalDTO> dtoList = mapper.Map<List<StockFundamental>, List<GetStockFundamentalDTO>>(list);
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GetStockMarketCapDTO>> GetStockMarketCap(string ticker)
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<StockMarketCap, GetStockMarketCapDTO>());
                Mapper mapper = new Mapper(configuration);

                List<StockMarketCap> list = await stockMapper.GetStockMarketCap(ticker);


                List<GetStockMarketCapDTO> dtoList = mapper.Map<List<StockMarketCap>, List<GetStockMarketCapDTO>>(list);
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GetStockMarketTRXDTO>> GetStockMarketTRX(string ticker)
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<StockMarketTRX, GetStockMarketTRXDTO>());
                Mapper mapper = new Mapper(configuration);

                List<StockMarketTRX> list = await stockMapper.GetStockMarketTRX(ticker);


                List<GetStockMarketTRXDTO> dtoList = mapper.Map<List<StockMarketTRX>, List<GetStockMarketTRXDTO>>(list);
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GetStockSectorTRXDTO>> GetStockSectorTRX(string ticker)
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<StockSectorTRX, GetStockSectorTRXDTO>());
                Mapper mapper = new Mapper(configuration);

                List<StockSectorTRX> list = await stockMapper.GetStockSectorTRX(ticker);


                List<GetStockSectorTRXDTO> dtoList = mapper.Map<List<StockSectorTRX>, List<GetStockSectorTRXDTO>>(list);
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
