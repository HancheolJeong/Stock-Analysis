using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappers
{
    public class IndexMapper : IIndexMapper
    {
        private string connectionString;

        public IndexMapper(string conn)
        {
            connectionString = conn;
        }

        public async Task<List<IndexData>> GetIndexData()
        {
            List<IndexData> list = new List<IndexData>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"
WITH LatestDates AS (
    SELECT 
        ticker, 
        MAX(trade_date) AS LatestDate
    FROM 
        stock.indexes_ohlcv
    GROUP BY 
        ticker
)
SELECT 
    i.ticker, 
    i.name, 
    o.closing_price, 
    o.market_value, 
    o.trading_volume, 
    o.trade_date
FROM 
    stock.indexes i
JOIN 
    stock.indexes_ohlcv o ON i.ticker = o.ticker
JOIN 
    LatestDates ld ON o.ticker = ld.ticker AND o.trade_date = ld.LatestDate
ORDER BY 
    i.ticker ASC;
;";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                IndexData index = new IndexData
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    name = reader.GetString(reader.GetOrdinal("name")),
                                    closing_price = reader.GetDecimal(reader.GetOrdinal("closing_price")),
                                    trading_volume = reader.GetInt64(reader.GetOrdinal("trading_volume")),
                                    market_value = reader.GetInt64(reader.GetOrdinal("market_value")),
                                    trade_date = DateOnly.FromDateTime((DateTime)reader["trade_date"])
                                };
                                list.Add(index);
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


        public async Task<List<IndexOHLCV>> GetIndexOHLCV(string ticker)
        {
            List<IndexOHLCV> list = new List<IndexOHLCV>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"SELECT * FROM stock.indexes_ohlcv WHERE ticker = @Ticker ORDER BY trade_date ASC";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Ticker", ticker);
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                IndexOHLCV indexOHLCV = new IndexOHLCV
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    trade_date = DateOnly.FromDateTime((DateTime)reader["trade_date"]),
                                    opening_price = reader.GetDecimal(reader.GetOrdinal("opening_price")),
                                    high_price = reader.GetDecimal(reader.GetOrdinal("high_price")),
                                    low_price = reader.GetDecimal(reader.GetOrdinal("low_price")),
                                    closing_price = reader.GetDecimal(reader.GetOrdinal("closing_price")),
                                    trading_volume = reader.GetInt64(reader.GetOrdinal("trading_volume")),
                                    transaction_amount = reader.GetInt64(reader.GetOrdinal("transaction_amount")),
                                    market_value = reader.GetInt64(reader.GetOrdinal("market_value")),
                                };
                                list.Add(indexOHLCV);
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



    }
}
