
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
    }
}