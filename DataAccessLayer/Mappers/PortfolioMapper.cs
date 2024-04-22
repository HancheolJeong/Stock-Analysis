using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappers
{
    public class PortfolioMapper : IPortfolioMapper
    {
        private string connectionString;
        public PortfolioMapper(string conn)
        {
            connectionString = conn;
        }

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
