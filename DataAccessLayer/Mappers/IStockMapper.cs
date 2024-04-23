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

        public Task<List<Stock>> GetAdvancedStockData();
        public Task<List<StockOHLCV>> GetStockOHLCV(string ticker);
        public Task<List<StockMarketCap>> GetStockMarketCap(string ticker);
        public Task<List<StockFundamental>> GetStockFundamental(string ticker);
        public Task<List<StockMarketTRX>> GetStockMarketTRX(string ticker);

        public Task<List<StockSectorTRX>> GetStockSectorTRX(string ticker);

    }
}
