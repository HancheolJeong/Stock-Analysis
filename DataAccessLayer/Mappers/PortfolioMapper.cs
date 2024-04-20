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
    }
}
