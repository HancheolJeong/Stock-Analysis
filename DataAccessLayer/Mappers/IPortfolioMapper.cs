using DataAccessLayer.Models;

namespace DataAccessLayer.Mappers
{
    public interface IPortfolioMapper
    {
        Task<bool> Create(Portfolio portfolio);
        public Task<List<Portfolio>> GetPortfolio(string email);

        public Task<bool> DeletePortfolio(int id);

	}
}