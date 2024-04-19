using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappers
{

    public class ProcCall : IProcCall
    {
        private string connectionString;


        public ProcCall(string conn)
        {
            this.connectionString = conn;
        }

        /// <summary>
        /// 프로시저를 호출하는데 사용되는 공통함수
        /// </summary>
        /// <param name="procedurename"></param>
        /// <param name="dc"></param>
        public async Task<DataTable> RequestProcedure(string procedurename, Dictionary<string, object> dc)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                SqlCommand command = new SqlCommand(procedurename, conn);
                command.CommandType = CommandType.StoredProcedure;

                //고정적이지않게 Dictionary
                foreach (var aw in dc)
                {
                    SqlParameter p1 = new SqlParameter();
                    p1.ParameterName = aw.Key;
                    p1.DbType = GetDbType(aw.Value.GetType());
                    p1.Direction = ParameterDirection.Input;
                    p1.Value = aw.Value;
                    command.Parameters.Add(p1);
                    //Console.WriteLine("키는" + aw.Key + "," + aw.Value);
                }
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(ds);

                //조회결과가 없을 경우
                if (ds.Tables.Count > 0)
                {
                    //조회된 행의수가 없을 경우
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        throw new Exception("DB fail");
                    }
                    //DB Catch에서 Error가 Select되서 넘어온경우.
                    if (ds.Tables[0].Rows[0][0].Equals("Error"))
                    {
                        throw new Exception("DB fail");
                    }
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception("DB fail");
                }

            }

        }
        public static DbType GetDbType(Type runtimeType)
        {
            var nonNullableType = Nullable.GetUnderlyingType(runtimeType);
            if (nonNullableType != null)
            {
                runtimeType = nonNullableType;
            }

            var templateValue = (Object)null;
            if (runtimeType.IsClass == false)
            {
                templateValue = Activator.CreateInstance(runtimeType);
            }

            var sqlParamter = new SqlParameter(parameterName: String.Empty, value: templateValue);

            return sqlParamter.DbType;
        }

    }

}
