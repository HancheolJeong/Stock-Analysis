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
    public class IndexService : IIndexService
    {
        IIndexMapper indexMapper;
        IMemoryCache _memoryCache;
        public IndexService(IMemoryCache memoryCache, IIndexMapper mapper)
        {
            indexMapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task LoadDataAsync()
        {

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<IndexData, GetIndexDTO>());

            List<IndexData> list = await indexMapper.GetIndexData();

            _memoryCache.Set("INDEX", list, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(25)));
        }

        public List<IndexData> GetIndex(int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<IndexData> indexes))
            {
                return indexes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<IndexData>();
        }

        public int GetIndexCount(int pageSize)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<IndexData> indexes))
            {
                int result = (int)Math.Ceiling(indexes.Count() / (double)pageSize);
                return result;
            }
            return 1;
        }

        public List<IndexData> SearchIndex(string query)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<IndexData> indexes))
            {
                return indexes.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList();
            }
            return new List<IndexData>();
        }

        public decimal SearchIndexByTicker(string ticker)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<IndexData> indexes))
            {
                var index = indexes
                            .Where(s => s.ticker == ticker)
                            .OrderByDescending(s => s.trade_date)
                            .FirstOrDefault();

                if (index != null)
                {
                    return index.closing_price;
                }
            }
            throw new Exception("No data found for the given ticker or cache is empty.");
        }
    }
}
