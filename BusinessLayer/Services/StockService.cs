using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class StockService : IStockService
    {
        IStockMapper stockMapper;
        public StockService(IStockMapper mapper)
        {
            stockMapper = mapper;
        }

        public async Task<List<GetStockDTO>> GetStockInfo()
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Stock, GetStockDTO>());

                List<Stock> list = await stockMapper.GetAll();


                Mapper mapper = new Mapper(configuration);
                List<GetStockDTO> dtoList = mapper.Map<List<Stock>, List<GetStockDTO>>(list);
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
