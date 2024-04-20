using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
