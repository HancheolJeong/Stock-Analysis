using BusinessLayer.DTO;
using DataAccessLayer.Models;

namespace BusinessLayer.Services
{
    public interface IStockService
    {
        Task<List<GetStockDTO>> GetStockInfo();

        public Task LoadDataAsync();

        public List<AdvancedStock> GetKOSPI(int pageNumber, int pageSize);

        public List<AdvancedStock> GetKOSDAQ(int pageNumber, int pageSize);

        public int GetKOSPICount(int pageSize);
        public int GetKOSDAQCount(int pageSize);

        public List<AdvancedStock> SearchKOSPI(string query);

        public List<AdvancedStock> SearchKOSDAQ(string query);
    }
}