
using DataAccessLayer.Models;

namespace BusinessLayer.Services
{
    public interface IIndexService
    {
        Task LoadDataAsync();
        public List<IndexData> SearchIndex(string query);
        public List<IndexData> GetIndex(int pageNumber, int pageSize);
        public int GetIndexCount(int pageSize);
        public decimal SearchIndexByTicker(string ticker);
    }
}