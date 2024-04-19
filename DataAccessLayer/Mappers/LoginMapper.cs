using DataAccessLayer.Models;
using System.Data.SqlClient;

namespace DataAccessLayer.Mappers
{
    public class LoginMapper : ILoginMapper
    {
        private string connectionString;
        public LoginMapper(string conn)
        {
            connectionString = conn;
        }




        public async Task<USER> Create(USER user)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = "INSERT INTO TBL_USER(userid, username, point) OUTPUT INSERTED.ID VALUES(@userid,@username,@point)";
                    sqlCommand.Parameters.AddWithValue("@userid", user.Userid);
                    sqlCommand.Parameters.AddWithValue("@username", user.Username);
                    sqlCommand.Parameters.AddWithValue("@point", user.Point);
                    int id = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
                    user.Id = id;


                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return user;
        }

        public async Task<List<USER>> GetAll()
        {
            try
            {
                List<USER> list = new List<USER>();
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = "SELECT * FROM TBL_USER";
                    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            USER usr = new USER
                            {

                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Userid = reader.GetString(reader.GetOrdinal("userid")),
                                Point = reader.GetInt32(reader.GetOrdinal("point"))
                            };
                            list.Add(usr);
                        }
                    }
                    reader.Close();
                }

                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
