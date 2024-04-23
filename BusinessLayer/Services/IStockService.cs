using BusinessLayer.DTO;
using DataAccessLayer.Models;

namespace BusinessLayer.Services
{
    public interface IStockService
    {

        public Task LoadDataAsync();
        public List<GetStockDTO> GetStock(string market, int pageNumber, int pageSize);

        public int GetKOSPICount(int pageSize);
        public int GetKOSDAQCount(int pageSize);
        public List<GetStockDTO> SearchStock(string market, string query);

        public List<GetStockDTO> GetTopStocksByValue(string marketKey, string sortBy, int n);
        public Task<List<GetStockOHLCVDTO>> GetStockOHLCV(string ticker);
        public Task<List<GetStockFundamentalDTO>> GetStockFundamental(string ticker);
        public Task<List<GetStockMarketCapDTO>> GetStockMarketCap(string ticker);
        public Task<List<GetStockMarketTRXDTO>> GetStockMarketTRX(string ticker);
        public Task<List<GetStockSectorTRXDTO>> GetStockSectorTRX(string ticker);

        public string GetNameByTicker(string market, string ticker);
        public int GetPriceByTicker(string market, string ticker);
        public int GetCountByDTO(ref List<GetStockDTO> stocks, int pageSize);
    }
}