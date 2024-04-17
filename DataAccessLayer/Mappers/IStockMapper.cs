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
    }
}
