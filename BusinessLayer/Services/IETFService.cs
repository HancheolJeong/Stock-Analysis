using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface IETFService
    {
        List<GetETFDTO> GetETF(int pageNumber, int pageSize); // 자주 접근하는 ETF 데이터를 캐시메모리에 적재하는 함수
		int GetETFCount(int pageSize); // 페이지를 기준으로 ETF 리스트를 반환하는 함수
		Task LoadDataAsync(); // ETF페이지 전체 페이지수를 반환하는 함수
		List<GetETFDTO> SearchETF(string query); // 검색 기준으로 ETF리스트를 반환하는 함수
		public string GetNameByTicker(string ticker); // ETF 리스트 티커를 기준으로 종목명을 반환하는 함수
		public int GetPriceByTicker(string ticker); // ETF 리스트 티커를 기준으로 가격을 반환하는 함수
		public int GetCountByDTO(ref List<GetETFDTO> stocks, int pageSize); // ETF리스트를 기준으로 최대 페이지수를 반환하는 함수
		public Task<List<GetETFOHLCVDTO>> GetETFOHLCVDTO(string ticker); // ETF OHLCV 리스트를 반환하는 함수
	}
}