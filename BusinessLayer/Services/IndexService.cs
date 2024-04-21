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
            Mapper mapper = new Mapper(configuration);

            List<IndexData> list = await indexMapper.GetIndexData();
            List<GetIndexDTO> dtoList = mapper.Map<List<IndexData>, List<GetIndexDTO>>(list);

            _memoryCache.Set("INDEX", dtoList, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(25)));
        }

        public List<GetIndexDTO> GetIndex(int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<GetIndexDTO> indexes))
            {
                return indexes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<GetIndexDTO>();
        }

        public int GetIndexCount(int pageSize)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<GetIndexDTO> indexes))
            {
                int result = (int)Math.Ceiling(indexes.Count() / (double)pageSize);
                return result;
            }
            return 1;
        }

        public int GetCountByDTO(ref List<GetIndexDTO> stocks, int pageSize)
        {
            if (stocks != null && stocks.Count > 0)
            {
                int result = (int)Math.Ceiling(stocks.Count / (double)pageSize);
                return result;
            }
            return 1;
        }

        public List<GetIndexDTO> SearchIndex(string query)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<GetIndexDTO> indexes))
            {
                return indexes.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList();
            }
            return new List<GetIndexDTO>();
        }

        public decimal SearchIndexByTicker(string ticker)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<GetIndexDTO> indexes))
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

        public async Task<List<GetIndexOHLCVDTO>> GetIndexOHLCV(string ticker)
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<IndexOHLCV, GetIndexOHLCVDTO>());
                Mapper mapper = new Mapper(configuration);

                List<IndexOHLCV> list = await indexMapper.GetIndexOHLCV(ticker);


                List<GetIndexOHLCVDTO> dtoList = mapper.Map<List<IndexOHLCV>, List<GetIndexOHLCVDTO>>(list);
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetNameByTicker(string ticker)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<GetIndexDTO> stocks))
            {
                // LINQ를 사용하여 ticker와 일치하는 첫 번째 주식을 찾고, 해당 주식의 이름을 반환
                var stock = stocks.FirstOrDefault(s => s.ticker == ticker);
                if (stock != null)
                {
                    return stock.name;
                }
            }
            return "Not found";
        }

    }
}
