using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CacheService
    {
        private readonly IMemoryCache _memoryCache;
        IStockMapper stockMapper;
        public CacheService(IMemoryCache memoryCache, IStockMapper mapper)
        {
            _memoryCache = memoryCache;
            stockMapper = mapper;
        }
        public async Task LoadDataAsync()
        {

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Stock, GetStockDTO>());

            List<Stock> list = await stockMapper.GetAdvancedStockData();
            _memoryCache.Set("stocks", list, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
        }

        public List<Stock> GetStocks(int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue("stocks", out List<Stock> stocks))
            {
                return stocks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<Stock>();
        }

        public List<Stock> SearchStocks(string query)
        {
            if (_memoryCache.TryGetValue("stocks", out List<Stock> stocks))
            {
                return stocks.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList();
            }
            return new List<Stock>();
        }
    }

}
