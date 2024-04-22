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
        public Task<List<GetStockOHLCVDTO>> GetStockOHLCV(string ticker);
        public Task<List<GetStockFundamentalDTO>> GetStockFundamental(string ticker);
        public Task<List<GetStockMarketCapDTO>> GetStockMarketCap(string ticker);
        public Task<List<GetStockMarketTRXDTO>> GetStockMarketTRX(string ticker);
        public Task<List<GetStockSectorTRXDTO>> GetStockSectorTRX(string ticker);

        public string GetNameByTicker(string market, string ticker);
        public int GetPriceByTicker(string market, string ticker);
        public int GetCountByDTO(ref List<GetAdvancedStockDTO> stocks, int pageSize);
    }
}