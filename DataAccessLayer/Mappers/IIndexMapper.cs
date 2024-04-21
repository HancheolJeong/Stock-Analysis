using DataAccessLayer.Models;

namespace DataAccessLayer.Mappers
{
    public interface IIndexMapper
    {
        Task<List<IndexData>> GetIndexData();

        Task<List<IndexOHLCV>> GetIndexOHLCV(string ticker);
    }
}