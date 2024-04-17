using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface IStockService
    {
        Task<List<GetStockDTO>> GetStockInfo();
    }
}