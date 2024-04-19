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

                List<Stock> list = await stockMapper.GetAll();


                Mapper mapper = new Mapper(configuration);
                List<GetStockDTO> dtoList = mapper.Map<List<Stock>, List<GetStockDTO>>(list);
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //In-memory에 데이터를 로드합니다.
        public async Task LoadDataAsync()
        {

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<AdvancedStock, GetAdvancedStockDTO>());

            List<AdvancedStock> list = await stockMapper.GetAdvancedStockData();
            var kospiStocks = list.Where(s => s.market == "KOSPI").ToList();
            var kosdaqStocks = list.Where(s => s.market == "KOSDAQ").ToList();

            _memoryCache.Set("KOSPI", kospiStocks, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
            _memoryCache.Set("KOSDAQ", kosdaqStocks, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
        }

        //데이터를 Inmomory에서 불러옵니다.
        public List<AdvancedStock> GetKOSPI(int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue("KOSPI", out List<AdvancedStock> stocks))
            {
                return stocks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<AdvancedStock>();
        }

        //데이터를 Inmomory에서 불러옵니다.
        public List<AdvancedStock> GetKOSDAQ(int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue("KOSDAQ", out List<AdvancedStock> stocks))
            {
                return stocks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<AdvancedStock>();
        }

        //페이지네이션이 가능한 페이지수를 리턴
        public int GetKOSPICount(int pageSize)
        {
            if (_memoryCache.TryGetValue("KOSPI", out List<AdvancedStock> stocks))
            {
                int result = (int)Math.Ceiling(stocks.Count() / (double)pageSize);
                return result;
            }
            return 1;
        }

        //페이지네이션이 가능한 페이지수를 리턴
        public int GetKOSDAQCount(int pageSize)
        {
            if (_memoryCache.TryGetValue("KOSDAQ", out List<AdvancedStock> stocks))
            {
                return stocks.Count() / pageSize;
            }
            return 1;
        }

        //데이터를 Inmemory에서 찾아서 불러옵니다.
        public List<AdvancedStock> SearchKOSPI(string query)
        {
            if (_memoryCache.TryGetValue("KOSPI", out List<AdvancedStock> stocks))
            {
                return stocks.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList();
            }
            return new List<AdvancedStock>();
        }

        //데이터를 Inmemory에서 찾아서 불러옵니다.
        public List<AdvancedStock> SearchKOSDAQ(string query)
        {
            if (_memoryCache.TryGetValue("KOSDAQ", out List<AdvancedStock> stocks))
            {
                return stocks.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList();
            }
            return new List<AdvancedStock>();
        }
    }
}
