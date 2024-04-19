using DataAccessLayer.Models;

namespace DataAccessLayer.Mappers
{
    public interface IIndexMapper
    {
        Task<List<IndexData>> GetIndexData();
    }
}