
using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface IIndexService
    {
        Task LoadDataAsync();
        public List<GetIndexDTO> SearchIndex(string query);
        public List<GetIndexDTO> GetIndex(int pageNumber, int pageSize);
        public int GetIndexCount(int pageSize);
        public decimal SearchIndexByTicker(string ticker);
        public Task<List<GetIndexOHLCVDTO>> GetIndexOHLCV(string ticker);

        public int GetCountByDTO(ref List<GetIndexDTO> stocks, int pageSize);
        public string GetNameByTicker(string ticker);
    }
}