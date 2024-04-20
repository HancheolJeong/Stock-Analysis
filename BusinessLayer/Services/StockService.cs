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
        IStockMapper stockMapper;
        IMemoryCache _memoryCache;
        public StockService(IMemoryCache memoryCache, IStockMapper mapper)
        {
            stockMapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<List<GetStockDTO>> GetStockInfo()
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Stock, GetStockDTO>());
                Mapper mapper = new Mapper(configuration);

                List<Stock> list = await stockMapper.GetAll();


                List<GetStockDTO> dtoList = mapper.Map<List<Stock>, List<GetStockDTO>>(list);
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task LoadDataAsync()
        {

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<AdvancedStock, GetAdvancedStockDTO>());
            Mapper mapper = new Mapper(configuration);

            List<AdvancedStock> list = await stockMapper.GetAdvancedStockData();
            List<GetAdvancedStockDTO> dtoList = mapper.Map<List<AdvancedStock>, List<GetAdvancedStockDTO>>(list);
            var kospiStocks = dtoList.Where(s => s.market == "KOSPI").ToList();
            var kosdaqStocks = dtoList.Where(s => s.market == "KOSDAQ").ToList();

            _memoryCache.Set("KOSPI", kospiStocks, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
            _memoryCache.Set("KOSDAQ", kosdaqStocks, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
        }

        public string GetNameByTicker(string market, string ticker)
        {
            if (_memoryCache.TryGetValue("KOSPI", out List<GetAdvancedStockDTO> stocks))
            {
                // LINQ를 사용하여 ticker와 일치하는 첫 번째 주식을 찾고, 해당 주식의 이름을 반환
                var stock = stocks.FirstOrDefault(s => s.ticker == ticker);
                if (stock != null)
                {
                    return stock.name;
                }
            }
            return "Not found"; // 일치하는 주식이 없을 경우 "Not found" 반환
        }

        public List<GetAdvancedStockDTO> GetKOSPI(int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue("KOSPI", out List<GetAdvancedStockDTO> stocks))
            {
                return stocks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<GetAdvancedStockDTO>();
        }

        public List<GetAdvancedStockDTO> GetKOSDAQ(int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue("KOSDAQ", out List<GetAdvancedStockDTO> stocks))
            {
                return stocks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<GetAdvancedStockDTO>();
        }

        public int GetKOSPICount(int pageSize)
        {
            if (_memoryCache.TryGetValue("KOSPI", out List<GetAdvancedStockDTO> stocks))
            {
                int result = (int)Math.Ceiling(stocks.Count() / (double)pageSize);
                return result;
            }
            return 1;
        }

        public int GetKOSDAQCount(int pageSize)
        {
            if (_memoryCache.TryGetValue("KOSDAQ", out List<GetAdvancedStockDTO> stocks))
            {
                return stocks.Count() / pageSize;
            }
            return 1;
        }

        public List<GetAdvancedStockDTO> SearchKOSPI(string query)
        {
            if (_memoryCache.TryGetValue("KOSPI", out List<GetAdvancedStockDTO> stocks))
            {
                return stocks.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList();
            }
            return new List<GetAdvancedStockDTO>();
        }

        public List<GetAdvancedStockDTO> SearchKOSDAQ(string query)
        {
            if (_memoryCache.TryGetValue("KOSDAQ", out List<GetAdvancedStockDTO> stocks))
            {
                return stocks.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList();
            }
            return new List<GetAdvancedStockDTO>();
        }
        public List<GetAdvancedStockDTO> GetTopStocksByValue(string marketKey, string sortBy, int n)
        {
            if (_memoryCache.TryGetValue(marketKey, out List<GetAdvancedStockDTO> stocks))
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
            return new List<GetAdvancedStockDTO>();
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
