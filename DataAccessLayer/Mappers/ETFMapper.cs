using DataAccessLayer.Models;
using System.Data.SqlClient;

namespace DataAccessLayer.Mappers
{
	public class ETFMapper : IETFMapper
    {
        private string connectionString;
        public ETFMapper(string conn)
        {
            connectionString = conn; // 의존성 주입
        }


        /// <summary>
        /// 데이터베이스에 ETF데이터를 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수
        /// </summary>
        /// <returns>ETF 리스트</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ETF>> GetETFData()
        {
            List<ETF> list = new List<ETF>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"
WITH ohlcv_date AS (
    SELECT ticker,MAX(trade_date) AS trade_date 
	FROM stock.etfs_ohlcv 
	GROUP BY ticker
),
ohlcv AS (
	SELECT o.ticker, o.closing_price, o.trading_volume, o.transaction_amount, o.trade_date 
	FROM stock.etfs_ohlcv o
	JOIN ohlcv_date od ON o.ticker = od.ticker AND o.trade_date = od.trade_date
)
SELECT e.ticker, e.name, o.closing_price, o.trading_volume, o.transaction_amount, o.trade_date 
FROM stock.etfs e
JOIN ohlcv o ON e.ticker = o.ticker
ORDER BY e.ticker ASC

;";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                ETF etf = new ETF
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    name = reader.GetString(reader.GetOrdinal("name")),
                                    closing_price = reader.GetInt32(reader.GetOrdinal("closing_price")),
                                    trading_volume = reader.GetInt64(reader.GetOrdinal("trading_volume")),
                                    transaction_amount = reader.GetInt64(reader.GetOrdinal("transaction_amount")),
                                    trade_date = DateOnly.FromDateTime((DateTime)reader["trade_date"])
                                };
                                list.Add(etf);
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
		/// 데이터베이스에 ETF OHLCV 데이터를 요청하고 해당 데이터를 리스트에 적재하고 반환하는 함수
		/// </summary>
		/// <param name="ticker">티커</param>
		/// <returns>ETF OHLCV 리스트</returns>
		/// <exception cref="Exception"></exception>
		public async Task<List<ETFOHLCV>> GetETFOHLCV(string ticker)
        {
            List<ETFOHLCV> list = new List<ETFOHLCV>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"SELECT * FROM stock.etfs_ohlcv WHERE ticker = @Ticker ORDER BY trade_date ASC";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Ticker", ticker);
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                ETFOHLCV etfOHLCV = new ETFOHLCV
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    trade_date = DateOnly.FromDateTime((DateTime)reader["trade_date"]),
                                    nav = reader.GetDecimal(reader.GetOrdinal("nav")),
                                    opening_price = reader.GetInt32(reader.GetOrdinal("opening_price")),
                                    high_price = reader.GetInt32(reader.GetOrdinal("high_price")),
                                    low_price = reader.GetInt32(reader.GetOrdinal("low_price")),
                                    closing_price = reader.GetInt32(reader.GetOrdinal("closing_price")),
                                    trading_volume = reader.GetInt64(reader.GetOrdinal("trading_volume")),
                                    transaction_amount = reader.GetInt64(reader.GetOrdinal("transaction_amount")),
                                    basic_index = reader.GetDecimal(reader.GetOrdinal("basic_index"))
                                };
                                list.Add(etfOHLCV);
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
