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
    }
}
