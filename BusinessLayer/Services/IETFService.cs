using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface IETFService
    {
        List<GetETFDTO> GetETF(int pageNumber, int pageSize);
        int GetETFCount(int pageSize);
        Task LoadDataAsync();
        List<GetETFDTO> SearchETF(string query);
    }
}