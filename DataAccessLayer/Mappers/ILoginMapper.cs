using DataAccessLayer.Models;

namespace DataAccessLayer.Mappers
{
    public interface ILoginMapper
    {
        Task<USER> Create(USER user);

        public Task<List<USER>> GetAll();
    }
}