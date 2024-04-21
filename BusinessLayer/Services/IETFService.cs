using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface IETFService
    {
        List<GetETFDTO> GetETF(int pageNumber, int pageSize);
        int GetETFCount(int pageSize);
        Task LoadDataAsync();
        List<GetETFDTO> SearchETF(string query);
        public string GetNameByTicker(string ticker);
        public int GetCountByDTO(ref List<GetETFDTO> stocks, int pageSize);
        public Task<List<GetETFOHLCVDTO>> GetETFOHLCVDTO(string ticker);
    }
}