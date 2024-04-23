using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLayer.Services
{
	public class StockService : IStockService
    {
        IStockMapper stockMapper; // Stock테이블 데이터베이스 매핑 인터페이스 선언
        IMemoryCache _memoryCache; 

        /// <summary>
        /// Stock 테이블과 관련된 비즈니스 로직 인스턴스를 초기화 합니다.
        /// </summary>
        /// <param name="memoryCache">매모리 캐시 객체</param>
        /// <param name="mapper">결과를 엔티티로 반환하는 객체</param> 
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

            List<Stock> list = await stockMapper.GetStockData();
            List<GetStockDTO> dtoList = mapper.Map<List<Stock>, List<GetStockDTO>>(list);
            var kospiStocks = dtoList.Where(s => s.market == "KOSPI").ToList(); // 시장이 KOSPI인 레코드로 분리해서 저장
            var kosdaqStocks = dtoList.Where(s => s.market == "KOSDAQ").ToList(); // 시장이 KOSDAQ인 레코드로 분리해서 저장
            _memoryCache.Remove("KOSPI");
            _memoryCache.Remove("KOSDAQ");
            _memoryCache.Set("KOSPI", kospiStocks, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(25))); // 캐시메모리에 저장 25시간동안 유효
            _memoryCache.Set("KOSDAQ", kosdaqStocks, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(25))); // 캐시메모리에 저장 25시간동안 유효
        }

        /// <summary>
        /// 티커로 종목명를 찾아서 반환하는 함수
        /// </summary>
        /// <param name="market">시장</param>
        /// <param name="ticker">티커</param>
        /// <returns>종목명</returns>
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
            return "";
        }

        /// <summary>
        /// 티커로 가격을 찾아서 반환하는 함수
        /// </summary>
        /// <param name="market">시장</param>
        /// <param name="ticker">티커</param>
        /// <returns>가격</returns>
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


        /// <summary>
        /// 리스트의 카운트와 페이지 사이즈를 기준으로 최대 페이지 수를 반환하는 함수
        /// </summary>
        /// <param name="stocks">주식 리스트</param>
        /// <param name="pageSize">페이지 사이즈</param>
        /// <returns>최대 페이지 수</returns>
        public int GetPageCountByDTO(ref List<GetStockDTO> stocks, int pageSize)
        {
            if (stocks != null && stocks.Count > 0)
            {
                int result = (int)Math.Ceiling(stocks.Count / (double)pageSize);
                return result;
            }
            return 1;
        }

        /// <summary>
        /// 주식 리스트를 페이지사이즈를 기준으로 나눠서 반환하는 함수
        /// </summary>
        /// <param name="market">시장</param>
        /// <param name="pageNumber">첫페이지 기준</param>
        /// <param name="pageSize">페이지수</param>
        /// <returns>주식리스트</returns>
        public List<GetStockDTO> GetStock(string market ,int pageNumber, int pageSize)
        {
            if (_memoryCache.TryGetValue(market, out List<GetStockDTO> stocks))
            {
                return stocks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return new List<GetStockDTO>();
        }


        /// <summary>
        /// 주식리스트의 최대 페이지수를 반환하는 함수 
        /// </summary>
        /// <param name="market">시장</param>
        /// <param name="pageSize">페이지수</param>
        /// <returns>최대 페이지수</returns>
        public int GetStockCount(string market, int pageSize)
        {
            if (_memoryCache.TryGetValue(market, out List<GetStockDTO> stocks))
            {
                int result = (int)Math.Ceiling(stocks.Count() / (double)pageSize);
                return result;
            }
            return 1;
        }


        /// <summary>
        /// 주식리스트에서 검색어에 해당하는 dto들을 필터링해서 반환하는 함수
        /// </summary>
        /// <param name="market">시장</param>
        /// <param name="query">검색어</param>
        /// <returns>필터링된 주식리스트</returns>
        public List<GetStockDTO> SearchStock(string market, string query)
        {
            if (_memoryCache.TryGetValue(market, out List<GetStockDTO> stocks))
            {
                return stocks.Where(s => s.name.Contains(query) || s.ticker.Contains(query)).ToList(); // 이름이나 티커가 검색어에 해당 될 경우
            }
            return new List<GetStockDTO>();
        }

        /// <summary>
        /// 주식리스트를 특정컬럼 기준으로 정렬하고 n만큼 랭킹안에 들어가는 요소만 남겨서 반환하는 함수
        /// </summary>
        /// <param name="market">시장</param>
        /// <param name="sortBy">정렬기준</param>
        /// <param name="n">dto의수</param>
        /// <returns>주식리스트</returns>
        /// <exception cref="ArgumentException"></exception>
        public List<GetStockDTO> GetTopStocksByValue(string market, string sortBy, int n)
        {
            if (_memoryCache.TryGetValue(market, out List<GetStockDTO> stocks))
            {
                switch (sortBy)
                {
                    case "marketCap": // 시가총액 순서로 정렬
                        return stocks.OrderByDescending(s => s.market_value).Take(n).ToList();
                    case "transactionAmount": //거래대금을 기준으로 정렬
                        return stocks.OrderByDescending(s => s.transaction_amount).Take(n).ToList();
                    default:
                        throw new ArgumentException("Invalid sort parameter. Use 'market_value' or 'transaction_amount'.");
                }
            }
            return new List<GetStockDTO>();
        }

        /// <summary>
        /// 주식 OHLCV 티커에 해당하는 리스트를 반환하는 함수
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <returns>주식 OHLCV 리스트</returns>
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

        /// <summary>
        /// 주식 Fundamental 티커에 해당하는 리스트를 반환하는 함수
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <returns>주식 Fundamental 리스트</returns>
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

        /// <summary>
        /// 주식 MarketCap 티커에 해당하는 리스트를 반환하는 함수
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <returns>주식 Fundamental 리스트</returns>
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

        /// <summary>
        /// 티커에 해당되는 주식 MarketTRX를 반환하는 함수
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <returns>주식 MarketTRX 리스트</returns>
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

        /// <summary>
        /// 티커에 해당하는 주식 SectorTrx를 반환하는 함수
        /// </summary>
        /// <param name="ticker">티커</param>
        /// <returns>주식 SectorTRX 리스트</returns>
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
