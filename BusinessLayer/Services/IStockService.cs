using BusinessLayer.DTO;
using DataAccessLayer.Models;

namespace BusinessLayer.Services
{
    public interface IStockService
    {
        Task<List<GetStockDTO>> GetStockInfo();

        public Task LoadDataAsync();

        public List<GetAdvancedStockDTO> GetKOSPI(int pageNumber, int pageSize);

        public List<GetAdvancedStockDTO> GetKOSDAQ(int pageNumber, int pageSize);

        public int GetKOSPICount(int pageSize);
        public int GetKOSDAQCount(int pageSize);

        public List<GetAdvancedStockDTO> SearchKOSPI(string query);

        public List<GetAdvancedStockDTO> SearchKOSDAQ(string query);

        public List<GetAdvancedStockDTO> GetTopStocksByValue(string marketKey, string sortBy, int n);
    }
}