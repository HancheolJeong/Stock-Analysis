using DataAccessLayer.Models;

namespace DataAccessLayer.Mappers
{
    public interface IETFMapper
    {
        Task<List<ETF>> GetETFData();
    }
}