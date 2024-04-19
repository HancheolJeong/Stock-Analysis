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
            List<Stock> list = new List<Stock>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT TOP 100 * FROM stock.stocks", sqlConnection))
                    {
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Stock stock = new Stock
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    name = reader.GetString(reader.GetOrdinal("name")),
                                    market = reader.GetString(reader.GetOrdinal("market"))
                                };
                                list.Add(stock);
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


        public async Task<List<Stock>> GetData(int pageNumber, int pageSize)
        {
            List<Stock> list = new List<Stock>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"
                        SELECT * FROM stock.stocks
                        ORDER BY name 
                        OFFSET @Offset ROWS FETCH NEXT @Fetch ROWS ONLY;";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                        sqlCommand.Parameters.AddWithValue("@Fetch", pageSize);
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Stock stock = new Stock
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    name = reader.GetString(reader.GetOrdinal("name")),
                                    market = reader.GetString(reader.GetOrdinal("market"))
                                };
                                list.Add(stock);
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
        public async Task<List<AdvancedStock>> GetAdvancedStockData()
        {
            List<AdvancedStock> list = new List<AdvancedStock>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"
WITH LatestTradeDates AS (
    SELECT 
        ticker, 
        MAX(trade_date) AS LatestTradeDate
    FROM 
        stock.stocks_ohlcv
    GROUP BY 
        ticker
),
Ohlcv AS (
    SELECT 
        o.ticker, 
        o.trade_date, 
        o.closing_price
    FROM 
        stock.stocks_ohlcv o
    JOIN 
        LatestTradeDates ltd ON o.ticker = ltd.ticker AND o.trade_date = ltd.LatestTradeDate
),
MarketCap AS (
    SELECT 
        m.ticker, 
        m.trade_date, 
        m.market_value, 
        m.trading_volume, 
        m.listed_stocks,
        m.transaction_amount
    FROM 
        stock.stocks_market_cap m
    JOIN 
        LatestTradeDates ltd ON m.ticker = ltd.ticker AND m.trade_date = ltd.LatestTradeDate
)
SELECT 
    s.name,
    s.ticker,
	s.market,
    o.closing_price,
    m.market_value,
    m.trading_volume,
    m.listed_stocks,
    m.transaction_amount,
	o.trade_date
FROM 
    stock.stocks s
JOIN 
    Ohlcv o ON s.ticker = o.ticker
JOIN 
    MarketCap m ON s.ticker = m.ticker;
;";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                AdvancedStock stock = new AdvancedStock
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    name = reader.GetString(reader.GetOrdinal("name")),
                                    market = reader.GetString(reader.GetOrdinal("market")),
                                    closing_price = reader.GetInt32(reader.GetOrdinal("closing_price")),
                                    market_value = reader.GetInt64(reader.GetOrdinal("market_value")),
                                    trading_volume = reader.GetInt64(reader.GetOrdinal("trading_volume")),
                                    transaction_amount = reader.GetInt64(reader.GetOrdinal("transaction_amount")),
                                    listed_stocks = reader.GetInt64(reader.GetOrdinal("listed_stocks")),
                                    trade_date = DateOnly.FromDateTime((DateTime)reader["trade_date"])
                                };
                                list.Add(stock);
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
