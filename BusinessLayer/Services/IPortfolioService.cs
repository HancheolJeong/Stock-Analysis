using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface IPortfolioService
    {
        Task<bool> CreatePortfolio(CreatePortfolioDTO createPortfolioDTO);
        public Task<List<GetPortfolioDTO>> GetPortfolio(string email);

        public int GetTotalPurchaseCost(ref List<GetPortfolioDTO> portfolios);

        public int GetTotalValuationProfitLoss(ref List<GetPortfolioDTO> portfolios);

        public int GetTotalValuation(ref List<GetPortfolioDTO> portfolios);

        public double GetTotalReturnPercentage(ref List<GetPortfolioDTO> portfolios);

        public Task<bool> DeletePortfolio(int id);

	}
}