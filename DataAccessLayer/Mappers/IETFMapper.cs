using DataAccessLayer.Models;

namespace DataAccessLayer.Mappers
{
    public interface IETFMapper
    {
        Task<List<ETF>> GetETFData(); // 데이터베이스에 ETF데이터를 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수
		public Task<List<ETFOHLCV>> GetETFOHLCV(string ticker); // 데이터베이스에 ETF OHLCV 데이터를 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수
	}
}