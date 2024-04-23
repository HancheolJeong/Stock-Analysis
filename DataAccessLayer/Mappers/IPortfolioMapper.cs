using DataAccessLayer.Models;

namespace DataAccessLayer.Mappers
{
    public interface IPortfolioMapper
    {
        Task<bool> Create(Portfolio portfolio); // 포트폴리오 데이터를 추가하고 해당 처리결과를 반환하는 함수
		public Task<List<Portfolio>> GetPortfolio(string email); // email을 기준으로 포트폴리오 데이터를 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수

		public Task<bool> DeletePortfolio(int id); // id를 기준으로 포트폴리오 레코드를 삭제하고 결과를 반환하는 함수

	}
}