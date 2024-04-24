using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface IPortfolioService
    {
        public Task<(bool, string?)> CreatePortfolio(CreatePortfolioDTO createPortfolioDTO); // 포트폴리오 구매목록을 추가하고 결과를 반환하는 함수
        public Task<(List<GetPortfolioDTO>, string?)> GetPortfolio(string email); // 이메일에 해당하는 포트폴리오를 가져오는 함수

        public int GetTotalPurchaseCost(ref List<GetPortfolioDTO> portfolios); // 총매수금액을 계산하고 결과를 반환하는 함수

        public int GetTotalValuationProfitLossCost(ref List<GetPortfolioDTO> portfolios); // 총평가손익을 계산하고 결과를 반환하는 함수

        public int GetTotalValuationCost(ref List<GetPortfolioDTO> portfolios); // 총평가금액을 계산하고 결과를 반환하는 함수

        public double GetTotalProfitLossPercentage(ref List<GetPortfolioDTO> portfolios); // 총평가손익률을 계산하고 결과를 반환하는 함수

		public Task<(bool, string?)> DeletePortfolio(int id); // id를 기준으로 포트폴리오 구매기록을 삭제하는 함수

	}
}