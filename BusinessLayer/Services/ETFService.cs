using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLayer.Services
{
	public class ETFService : IETFService
    {
        IETFMapper etfMapper;
        IMemoryCache _memoryCache;

        /// <summary>
        /// ETF 테이블과 관련된 비즈니스 로직 인스턴스를 초기화
        /// </summary>
        /// <param name="memoryCache">캐시메모리</param>
        /// <param name="mapper">엔티티 반환하는 함수 주입</param>
        public ETFService(IMemoryCache memoryCache, IETFMapper mapper)
        {
            etfMapper = mapper;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 자주 접근하는 ETF 데이터를 캐시메모리에 적재하는 함수
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataAsync()
        {

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<ETF, GetETFDTO>());
            Mapper mapper = new Mapper(configuration);

            List<ETF> list = await etfMapper.GetETFData();
            List<GetETFDTO> dtoList = mapper.Map<List<ETF>, List<GetETFDTO>>(list);

            _memoryCache.Set("ETF", dtoList, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(25)));
        }

        /// <summary>
        /// 페이지를 기준으로 ETF 리스트를 반환하는 함수
        /// </summary>
        /// <param name="pageNumber">페이지 시작 번호</param>
        /// <param name="pageSize">페이지 크기</param>
        /// <returns></returns>
        public List<GetETFDTO> GetETF(int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue("ETF", out List<GetETFDTO> etfs))
            {
                return etfs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<GetETFDTO>();
        }

        /// <summary>
        /// ETF페이지 전체 페이지수를 반환하는 함수
        /// </summary>
        /// <param name="pageSize">페이지 크기</param>
        /// <returns>페이지수</returns>
        public int GetETFCount(int pageSize)
        {
            if (_memoryCache.TryGetValue("ETF", out List<GetETFDTO> indexes))
            {
                int result = (int)Math.Ceiling(indexes.Count() / (double)pageSize);
                return result;
            }
            return 1;
        }

        /// <summary>
        /// 검색 기준으로 ETF리스트를 반환하는 함수
        /// </summary>
        /// <param name="query">검색어</param>
        /// <returns>ETF 리스트</returns>
        public List<GetETFDTO> SearchETF(string query)
        {
            if (_memoryCache.TryGetValue("ETF", out List<GetETFDTO> etfs))
            {
                return etfs.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList(); // name또는 ticker가 검색어에 포함 될 경우
            }
            return new List<GetETFDTO>();
        }

        /// <summary>
        /// ETF 리스트 티커를 기준으로 종목명을 반환하는 함수
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <returns>종목명</returns>
        public string GetNameByTicker(string ticker)
        {
            if (_memoryCache.TryGetValue("ETF", out List<GetETFDTO> stocks))
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

        /// <summary>
        /// ETF 리스트 티커를 기준으로 가격을 반환하는 함수
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <returns>가격</returns>
        public int GetPriceByTicker(string ticker)
        {
            if (_memoryCache.TryGetValue("ETF", out List<GetETFDTO> stocks))
            {
                // LINQ를 사용하여 ticker와 일치하는 첫 번째 주식을 찾고, 해당 주식의 이름을 반환
                var stock = stocks.FirstOrDefault(s => s.ticker == ticker);
                if (stock != null)
                {
                    return stock.closing_price;
                }
            }
            return 0;
        }

        /// <summary>
        /// ETF리스트를 기준으로 최대 페이지수를 반환하는 함수
        /// </summary>
        /// <param name="stocks">ETF리스트</param>
        /// <param name="pageSize">페이지 크기</param>
        /// <returns>ETF 리스트</returns>
        public int GetCountByDTO(ref List<GetETFDTO> stocks, int pageSize)
        {
            if (stocks != null && stocks.Count > 0)
            {
                int result = (int)Math.Ceiling(stocks.Count / (double)pageSize);
                return result;
            }
            return 1;
        }

        /// <summary>
        /// ETF OHLCV 리스트를 반환하는 함수
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <returns>ETF OHLCV 리스트</returns>
        public async Task<List<GetETFOHLCVDTO>> GetETFOHLCVDTO(string ticker)
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<ETFOHLCV, GetETFOHLCVDTO>());
                Mapper mapper = new Mapper(configuration);

                List<ETFOHLCV> list = await etfMapper.GetETFOHLCV(ticker);


                List<GetETFOHLCVDTO> dtoList = mapper.Map<List<ETFOHLCV>, List<GetETFOHLCVDTO>>(list);
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
