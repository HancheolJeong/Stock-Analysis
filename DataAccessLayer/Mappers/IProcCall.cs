using System.Data;

namespace DataAccessLayer.Mappers
{
    public interface IProcCall // 프로시저 관리 인터페이스
    {
        Task<DataTable> RequestProcedure(string procedurename, Dictionary<string, object> dc); // 공통 프로시저 처리 함수
    }
}