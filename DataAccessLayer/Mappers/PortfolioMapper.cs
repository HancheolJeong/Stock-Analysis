using DataAccessLayer.Models;
using System.Data.SqlClient;

namespace DataAccessLayer.Mappers
{
	public class PortfolioMapper : IPortfolioMapper
    {
        private string connectionString;
        public PortfolioMapper(string conn)
        {
            connectionString = conn;
        }

        /// <summary>
        /// 포트폴리오 데이터를 추가하고 해당 처리결과를 반환하는 함수
        /// </summary>
        /// <param name="portfolio">포트폴리오 엔티티</param>
        /// <returns>처리결과</returns>
        public async Task<bool> Create(Portfolio portfolio)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"
                INSERT INTO stock.portfolio(ticker, market, amount, unit_price, email) 
                VALUES(@ticker, @market, @amount, @unit_price, @email)";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ticker", portfolio.ticker);
                        sqlCommand.Parameters.AddWithValue("@market", portfolio.market);
                        sqlCommand.Parameters.AddWithValue("@amount", portfolio.amount);
                        sqlCommand.Parameters.AddWithValue("@unit_price", portfolio.unit_price);
                        sqlCommand.Parameters.AddWithValue("@email", portfolio.email);

                        int rowsAffected = await sqlCommand.ExecuteNonQueryAsync();
                        return rowsAffected == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// email을 기준으로 포트폴리오 데이터를 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Portfolio>> GetPortfolio(string email)
        {
            List<Portfolio> list = new List<Portfolio>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"SELECT * FROM stock.portfolio WHERE email = @email ORDER BY create_dt ASC";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@email", email);
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Portfolio portfolio = new Portfolio
                                {
                                    id = reader.GetInt32(reader.GetOrdinal("id")),
                                    email = reader.GetString(reader.GetOrdinal("email")),
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    market = reader.GetString(reader.GetOrdinal("market")),
                                    amount = reader.GetInt32(reader.GetOrdinal("amount")),
                                    unit_price = reader.GetInt32(reader.GetOrdinal("unit_price")),
                                    create_dt = DateOnly.FromDateTime((DateTime)reader["create_dt"])
                                };
                                list.Add(portfolio);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Ideally, log this exception
                throw new Exception("An error occurred while retrieving stocks.", ex);
            }
            return list;
        }

        /// <summary>
        /// id를 기준으로 포트폴리오 레코드를 삭제하고 결과를 반환하는 함수
        /// </summary>
        /// <param name="id">고유 ID</param>
        /// <returns>처리결과</returns>
        /// <exception cref="Exception"></exception>
		public async Task<bool> DeletePortfolio(int id)
		{
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(connectionString))
				{
					await sqlConnection.OpenAsync();
					string query = "DELETE FROM stock.portfolio WHERE id = @id";

					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@id", id);

						int rowsAffected = await sqlCommand.ExecuteNonQueryAsync();
						return rowsAffected == 1;  
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while deleting the stock.", ex);
			}
		}


	}
}
