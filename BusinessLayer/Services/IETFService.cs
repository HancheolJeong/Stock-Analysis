using DataAccessLayer.Models;

namespace BusinessLayer.Services
{
    public interface IETFService
    {
        List<ETF> GetETF(int pageNumber, int pageSize);
        int GetETFCount(int pageSize);
        Task LoadDataAsync();
        List<ETF> SearchETF(string query);
    }
}