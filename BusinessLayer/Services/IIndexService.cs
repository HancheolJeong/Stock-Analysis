
using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface IIndexService
    {
        Task LoadDataAsync(); // 캐시메모리 데이터를 적재하는 함수
        public List<GetIndexDTO> SearchIndex(string query); // 검색 대상 인덱스 리스트를 반환하는 함수
        public List<GetIndexDTO> GetIndex(int pageNumber, int pageSize); // 페이지를 기준으로 인덱스 리스트를 반환하는 함수
        public int GetIndexCount(int pageSize); // 인덱스 리스트의 최대 페이지수를 반환하는 함수
        public decimal SearchIndexByTicker(string ticker); // 인덱스 리스트의 최근날짜 종가를 반환하는 함수
        public Task<List<GetIndexOHLCVDTO>> GetIndexOHLCV(string ticker); // 인덱스 OHLCV를 반환하는 함수

        public int GetCountByDTO(ref List<GetIndexDTO> stocks, int pageSize); // 인덱스 리스트의 최대 페이지수를 반환하는 함수
        public string GetNameByTicker(string ticker); // 인덱스 리스트의 티커를 기준으로 종목명을 반환하는 함수
    }
}