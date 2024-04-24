using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;

namespace BusinessLayer.Services
{
	public class PortfolioService : IPortfolioService
    {
        IPortfolioMapper _portfolioMapper;

        /// <summary>
        /// Portfolio 테이블과 관련된 비즈니스 로직 인스턴스를 초기화
        /// </summary>
        /// <param name="mapper">결과를 엔티티로 반환하는 객체를 주입</param>
        public PortfolioService(IPortfolioMapper mapper)
        {
            _portfolioMapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createPortfolioDTO"></param>
        /// <returns></returns>
        public async Task<(bool, string?)> CreatePortfolio(CreatePortfolioDTO createPortfolioDTO)
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<CreatePortfolioDTO, Portfolio>());
                Mapper mapper = new Mapper(configuration);
                Portfolio portfolio = mapper.Map<CreatePortfolioDTO, Portfolio>(createPortfolioDTO);
                bool result = await _portfolioMapper.Create(portfolio);
                return (result, null);
            }
            catch (Exception ex)
            {
                // 예외 로깅
                string err = $"예외 발생 : BusinessLayer/Services/PortfolioService/(Function)CreatePortfolio {ex.Message}";
                return (false, err);
            }
        }

        /// <summary>
        /// Portfolio를 요청하고 GetPortfolioDTO으로 매핑하고 리턴하는 함수
        /// </summary>
        /// <param name="email">사용자 이메일</param>
        /// <returns>List<GetPortfolioDTO></returns>
        public async Task<(List<GetPortfolioDTO>, string?)> GetPortfolio(string email)
        {
            try
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Portfolio, GetPortfolioDTO>()
                       .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.amount * src.unit_price));
                });

                Mapper mapper = new Mapper(configuration);

                List<Portfolio> list = await _portfolioMapper.GetPortfolio(email);
                List<GetPortfolioDTO> dtoList = mapper.Map<List<Portfolio>, List<GetPortfolioDTO>>(list);
                return (dtoList, null);
            }
            catch (Exception ex)
            {
                string err = $"예외 발생 : BusinessLayer/Services/PortfolioService/(Function)GetPortfolio {ex.Message}";
                return (new List<GetPortfolioDTO>(), err);
            }
        }


        /// <summary>
        /// 총매수금액을 계산하고 결과를 반환하는 함수
        /// </summary>
        /// <param name="portfolios"></param>
        /// <returns>총매수금액</returns>
        public int GetTotalPurchaseCost(ref List<GetPortfolioDTO> portfolios)
        {
            int totalPurchaseCost = portfolios.Sum(p => p.amount * p.unit_price); 
            return totalPurchaseCost;
        }

        /// <summary>
        /// 총평가손익을 계산하고 결과를 반환하는 함수
        /// </summary>
        /// <param name="portfolios"></param>
        /// <returns>총평가손익</returns>
        public int GetTotalValuationProfitLossCost(ref List<GetPortfolioDTO> portfolios)
        {
            int totalProfitLoss = portfolios.Sum(p => ((int)p.current_price - p.unit_price) * p.amount);
            return totalProfitLoss;
        }

        /// <summary>
        /// 총평가금액을 계산하고 결과를 반환하는 함수
        /// </summary>
        /// <param name="portfolios"></param>
        /// <returns>총평가금액</returns>
        public int GetTotalValuationCost(ref List<GetPortfolioDTO> portfolios)
        {
            int totalValuation = portfolios.Sum(p => (int)p.current_price * p.amount);
            return totalValuation;
        }

        /// <summary>
        /// 총평가손익률을 계산하고 결과를 반환하는 함수
        /// </summary>
        /// <param name="portfolios"></param>
        /// <returns>총평가손익</returns>
        public double GetTotalProfitLossPercentage(ref List<GetPortfolioDTO> portfolios)
        {
            double totalPurchaseCost = GetTotalPurchaseCost(ref portfolios); // 총매수금액
            double totalValuation = GetTotalValuationCost(ref portfolios); // 총평가금액
            if (totalPurchaseCost == 0) return 0; // 매수한 금액이 없다면 0으로 반환
            double totalReturnPercentage = (totalValuation - totalPurchaseCost) / totalPurchaseCost * 100; // (총평가금액 - 총매수금액) / 총매수금액 * 100
            return totalReturnPercentage;
        }

        /// <summary>
        /// id를 기준으로 구매기록을 삭제하는 함수
        /// </summary>
        /// <param name="id">portfolio테이블의 id</param>
        /// <returns>처리결과</returns>
		public async Task<(bool, string?)> DeletePortfolio(int id)
		{
			try
			{
                bool result = await _portfolioMapper.DeletePortfolio(id);
				return (result, null);
			}
			catch (Exception ex)
			{
				string err = $"예외 발생 : BusinessLayer/Services/PortfolioService/(Function)DeletePortfolio {ex.Message}";
				return (false, err);
			}
		}

	}
}
