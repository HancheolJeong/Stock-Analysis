using DataAccessLayer.Models;

namespace DataAccessLayer.Mappers
{
	public interface IStockMapper
    {

        public Task<List<Stock>> GetStockData(); // Stock테이블 데이터를 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수
		public Task<List<StockOHLCV>> GetStockOHLCV(string ticker); // Stock OHLCV 테이블 데이터를 티커를 기준으로 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수
		public Task<List<StockMarketCap>> GetStockMarketCap(string ticker); // Stock MarketCap 테이블 데이터를 티커 기준으로 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수
		public Task<List<StockFundamental>> GetStockFundamental(string ticker); // Stock Fundamental 테이블 데이터를 티커를 기준으로 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수
		public Task<List<StockMarketTRX>> GetStockMarketTRX(string ticker); // Stock MarketTRX 테이블 데이터릴 티커로 기준으로 요청하고 해당 데이터를 리스틍에 적재하고 반환하는 함수
		public Task<List<StockSectorTRX>> GetStockSectorTRX(string ticker); // Stock SectorTRX 테이블 데이터릴 티커로 기준으로 요청하고 해당 데이터를 리스틍에 적재하고 반환하는 함수

	}
}
