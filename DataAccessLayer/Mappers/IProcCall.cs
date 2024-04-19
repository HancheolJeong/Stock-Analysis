using System.Data;

namespace DataAccessLayer.Mappers
{
    public interface IProcCall
    {
        Task<DataTable> RequestProcedure(string procedurename, Dictionary<string, object> dc);
    }
}