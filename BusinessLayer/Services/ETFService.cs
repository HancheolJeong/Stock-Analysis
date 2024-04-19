using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ETFService : IETFService
    {
        IETFMapper etfMapper;
        IMemoryCache _memoryCache;
        public ETFService(IMemoryCache memoryCache, IETFMapper mapper)
        {
            etfMapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task LoadDataAsync()
        {

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<ETF, GetETFDTO>());

            List<ETF> list = await etfMapper.GetETFData();

            _memoryCache.Set("ETF", list, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(25)));
        }

        //데이터를 Inmomory에서 불러옵니다.
        public List<ETF> GetETF(int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue("ETF", out List<ETF> etfs))
            {
                return etfs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<ETF>();
        }

        public int GetETFCount(int pageSize)
        {
            if (_memoryCache.TryGetValue("ETF", out List<ETF> indexes))
            {
                int result = (int)Math.Ceiling(indexes.Count() / (double)pageSize);
                return result;
            }
            return 1;
        }

        public List<ETF> SearchETF(string query)
        {
            if (_memoryCache.TryGetValue("ETF", out List<ETF> etfs))
            {
                return etfs.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList();
            }
            return new List<ETF>();
        }
    }
}
