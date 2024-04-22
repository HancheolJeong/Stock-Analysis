using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;

namespace BusinessLayer.Services
{
    public class PortfolioService : IPortfolioService
    {
        IPortfolioMapper _portfolioMapper;
        public PortfolioService(IPortfolioMapper mapper)
        {
            _portfolioMapper = mapper;
        }
        public async Task<bool> CreatePortfolio(CreatePortfolioDTO createPortfolioDTO)
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<CreatePortfolioDTO, Portfolio>());
                Mapper mapper = new Mapper(configuration);
                Portfolio portfolio = mapper.Map<CreatePortfolioDTO, Portfolio>(createPortfolioDTO);
                bool result = await _portfolioMapper.Create(portfolio);
                return result;
            }
            catch (Exception ex)
            {
                // 예외 로깅
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Portfolio를 요청하고 GetPortfolioDTO으로 매핑하고 리턴하는 함수
        /// </summary>
        /// <param name="email">사용자 이메일</param>
        /// <returns>List<GetPortfolioDTO></returns>
        public async Task<List<GetPortfolioDTO>> GetPortfolio(string email)
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
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public int GetTotalPurchaseCost(ref List<GetPortfolioDTO> portfolios)
        {
            int totalPurchaseCost = portfolios.Sum(p => p.amount * p.unit_price);
            return totalPurchaseCost;
        }

        public int GetTotalValuationProfitLoss(ref List<GetPortfolioDTO> portfolios)
        {
            int totalProfitLoss = portfolios.Sum(p => ((int)p.current_price - p.unit_price) * p.amount);
            return totalProfitLoss;
            return 0;
        }

        public int GetTotalValuation(ref List<GetPortfolioDTO> portfolios)
        {
            int totalValuation = portfolios.Sum(p => (int)p.current_price * p.amount);
            return totalValuation;
            return 0;
        }

        public double GetTotalReturnPercentage(ref List<GetPortfolioDTO> portfolios)
        {
            double totalPurchaseCost = GetTotalPurchaseCost(ref portfolios);
            double totalValuation = GetTotalValuation(ref portfolios);
            if (totalPurchaseCost == 0) return 0; // Avoid division by zero
            double totalReturnPercentage = (totalValuation - totalPurchaseCost) / totalPurchaseCost * 100;
            return totalReturnPercentage;
        }

		public async Task<bool> DeletePortfolio(int id)
		{
			try
			{
				return await _portfolioMapper.DeletePortfolio(id);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error deleting portfolio with ID {id}: {ex.Message}");
				return false;
			}
		}

	}
}
