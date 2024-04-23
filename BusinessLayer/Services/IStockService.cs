using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
	public interface IStockService
    {

        public Task LoadDataAsync(); // 캐시메모리 데이터를 불러오는 함수
        public List<GetStockDTO> GetStock(string market, int pageNumber, int pageSize); // 주식 정보를 가져오는 함수

        public int GetStockCount(string market, int pageSize); // 주식 리스트 레코드수를 반환하는 함수
        public List<GetStockDTO> SearchStock(string market, string query); // 검색 대상 주식 리스트를 반환하는 함수

        public List<GetStockDTO> GetTopStocksByValue(string market, string sortBy, int n); // sortBy기준으로 정렬하고 n만큼 랭킹안에 들어가는 주식 리스트를 반환하는 함수
        public Task<List<GetStockOHLCVDTO>> GetStockOHLCV(string ticker); // 주식 OHLCV 리스트를 반환하는 함수
        public Task<List<GetStockFundamentalDTO>> GetStockFundamental(string ticker); // 주식 Fundamental 리스트를 반환하는 함수
        public Task<List<GetStockMarketCapDTO>> GetStockMarketCap(string ticker); // 주식 MarketCap 리스트를 반환하는 함수
        public Task<List<GetStockMarketTRXDTO>> GetStockMarketTRX(string ticker); // 주식 MarketTRX 리스트를 반환하는 함수
        public Task<List<GetStockSectorTRXDTO>> GetStockSectorTRX(string ticker); // 주식 SectorTRX 리스트를 반환하는 함수

        public string GetNameByTicker(string market, string ticker); // 티커에 해당하는 종목명을 반환하는 함수
        public int GetPriceByTicker(string market, string ticker); // 티커에 해당하는 종가를 반환하는 함수
        public int GetPageCountByDTO(ref List<GetStockDTO> stocks, int pageSize); // pagesize으로 페이지수를 리턴하는 함수
    }
}