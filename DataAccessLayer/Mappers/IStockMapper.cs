using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappers
{
    public interface IStockMapper
    {
        Task<Stock> Create(Stock stock);

        public Task<List<Stock>> GetAll();

        public Task<List<Stock>> GetData(int pageNumber, int pageSize);

        public Task<List<AdvancedStock>> GetAdvancedStockData();
    }
}
