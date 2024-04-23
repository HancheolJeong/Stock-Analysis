using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLayer.Services
{
	public class IndexService : IIndexService
    {
        IIndexMapper indexMapper;
        IMemoryCache _memoryCache;

        /// <summary>
        /// Indexes 테이블과 관련된 비즈니스 로직 인스턴스를 초기화
        /// </summary>
        /// <param name="memoryCache">캐시메모리</param>
        /// <param name="mapper">결과를 엔티티로 반환하는 객체를 주입</param>
        public IndexService(IMemoryCache memoryCache, IIndexMapper mapper)
        {
            indexMapper = mapper;
            _memoryCache = memoryCache;
        }

		/// <summary>
		/// 자주 접근하는 index 데이터를 캐시메모리에 적재하는 함수
		/// </summary>
		public async Task LoadDataAsync()
        {

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<IndexData, GetIndexDTO>());
            Mapper mapper = new Mapper(configuration);

            List<IndexData> list = await indexMapper.GetIndexData();
            List<GetIndexDTO> dtoList = mapper.Map<List<IndexData>, List<GetIndexDTO>>(list);

            _memoryCache.Set("INDEX", dtoList, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(25)));
        }

        /// <summary>
        /// 페이지를 기준으로 인덱스 리스트를 반환하는 함수
        /// </summary>
        /// <param name="pageNumber">페이지 시작번호</param>
        /// <param name="pageSize">페이지 수</param>
        /// <returns>인덱스 테이블</returns>
        public List<GetIndexDTO> GetIndex(int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<GetIndexDTO> indexes))
            {
                return indexes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<GetIndexDTO>();
        }

        /// <summary>
        /// 인덱스 테이블 최대 페이지를 반환하는 함수
        /// </summary>
        /// <param name="pageSize">페이지 수</param>
        /// <returns>페이지 번호</returns>
        public int GetIndexCount(int pageSize)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<GetIndexDTO> indexes))
            {
                int result = (int)Math.Ceiling(indexes.Count() / (double)pageSize);
                return result;
            }
            return 1;
        }

        /// <summary>
        /// 인덱스 테이블의 최대 페이지를 반환하는 함수
        /// </summary>
        /// <param name="stocks">인덱스 테이블</param>
        /// <param name="pageSize">페이지 수</param>
        /// <returns>최대 페이지수</returns>
        public int GetCountByDTO(ref List<GetIndexDTO> stocks, int pageSize)
        {
            if (stocks != null && stocks.Count > 0)
            {
                int result = (int)Math.Ceiling(stocks.Count / (double)pageSize);
                return result;
            }
            return 1;
        }

        /// <summary>
        /// 검색어를 기준으로 필터링된 인덱스 리스트를 반환하는 함수
        /// </summary>
        /// <param name="query">검색어</param>
        /// <returns>인덱스 리스트</returns>
        public List<GetIndexDTO> SearchIndex(string query)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<GetIndexDTO> indexes))
            {
                return indexes.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList(); //종목명이나 티커가 검색어에 포함된 경우
            }
            return new List<GetIndexDTO>();
        }

        /// <summary>
        /// 티커에 해당하는 가장최근날짜의 가격을 반환하는 함수
        /// </summary>
        /// <param name="ticker"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public decimal SearchIndexByTicker(string ticker)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<GetIndexDTO> indexes))
            {
                var index = indexes
                            .Where(s => s.ticker == ticker) 
                            .OrderByDescending(s => s.trade_date) // 날짜를 기준을 정렬
                            .FirstOrDefault();

                if (index != null)
                {
                    return index.closing_price;
                }
            }
            throw new Exception("No data found for the given ticker or cache is empty.");
        }

        /// <summary>
        /// 인덱스 OHLCV 리스트를 반환하는 함수
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <returns>인덱스 OHLCV 리스트</returns>
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

        /// <summary>
        /// 티커에 해당하는 종목명을 반환하는 함수
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <returns>종목명</returns>
        public string GetNameByTicker(string ticker)
        {
            if (_memoryCache.TryGetValue("INDEX", out List<GetIndexDTO> stocks))
            {
                var stock = stocks.FirstOrDefault(s => s.ticker == ticker);
                if (stock != null)
                {
                    return stock.name;
                }
            }
            return "";
        }

    }
}
