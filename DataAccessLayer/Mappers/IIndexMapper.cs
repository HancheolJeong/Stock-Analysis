using DataAccessLayer.Models;

namespace DataAccessLayer.Mappers
{
    public interface IIndexMapper
    {
        Task<List<IndexData>> GetIndexData(); // 데이터베이스에 Index 데이터를 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수

		Task<List<IndexOHLCV>> GetIndexOHLCV(string ticker); // 데이터베이스에 INDEX OHLCV 데이터를 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수
	}
}