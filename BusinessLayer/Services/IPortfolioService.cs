using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface IPortfolioService
    {
        Task<bool> CreatePortfolio(CreatePortfolioDTO createPortfolioDTO);
    }
}