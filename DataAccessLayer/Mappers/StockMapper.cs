using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappers
{
    public class StockMapper : IStockMapper
    {
        private string connectionString;
        public StockMapper(string conn) {
            connectionString = conn;
        }

        /*
         * 티커 생성 요청
         */
        public async Task<Stock> Create(Stock stock)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = "INSERT INTO stocks(ticker, name, market) OUTPUT INSERTED.ID VALUES(@ticker,@name,@market)";
                    sqlCommand.Parameters.AddWithValue("@ticker", stock.ticker);
                    sqlCommand.Parameters.AddWithValue("@name", stock.name);
                    sqlCommand.Parameters.AddWithValue("@market", stock.market);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return stock;
        }

        /*
         * 모든 티커 조회
         */
        public async Task<List<Stock>> GetAll()
        {
            try
            {
                List<Stock> list = new List<Stock>();
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = "SELECT TOP 100 * FROM Stocks";
                    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Stock stock = new Stock
                            {

                                ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                market = reader.GetString(reader.GetOrdinal("market")),
                            };
                            list.Add(stock);
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
