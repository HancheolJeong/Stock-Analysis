using DataAccessLayer.Models;

namespace DataAccessLayer.Mappers
{
    public interface IPortfolioMapper
    {
        Task<bool> Create(Portfolio portfolio);
    }
}