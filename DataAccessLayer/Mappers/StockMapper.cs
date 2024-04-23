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

        public async Task<List<Stock>> GetStockData()
        {
            List<Stock> list = new List<Stock>();
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
                                Stock stock = new Stock
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

        public async Task<List<StockOHLCV>> GetStockOHLCV(string ticker)
        {
            List<StockOHLCV> list = new List<StockOHLCV>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"SELECT * FROM stock.stocks_ohlcv WHERE ticker = @Ticker ORDER BY trade_date ASC";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Ticker", ticker);
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                StockOHLCV ohlcv = new StockOHLCV
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    trade_date = DateOnly.FromDateTime((DateTime)reader["trade_date"]),
                                    opening_price = reader.GetInt32(reader.GetOrdinal("opening_price")),
                                    high_price = reader.GetInt32(reader.GetOrdinal("high_price")),
                                    low_price = reader.GetInt32(reader.GetOrdinal("low_price")),
                                    closing_price = reader.GetInt32(reader.GetOrdinal("closing_price")),
                                    trading_volume = reader.GetInt64(reader.GetOrdinal("trading_volume")),
                                };
                                list.Add(ohlcv);
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

        public async Task<List<StockMarketCap>> GetStockMarketCap(string ticker)
        {
            List<StockMarketCap> list = new List<StockMarketCap>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"SELECT * FROM stock.stocks_market_cap WHERE ticker = @Ticker ORDER BY trade_date ASC";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Ticker", ticker);
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                StockMarketCap marketCap = new StockMarketCap
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    trade_date = DateOnly.FromDateTime((DateTime)reader["trade_date"]),
                                    market_value = reader.GetInt64(reader.GetOrdinal("market_value")),
                                    trading_volume = reader.GetInt64(reader.GetOrdinal("trading_volume")),
                                    transaction_amount = reader.GetInt64(reader.GetOrdinal("transaction_amount")),
                                    listed_stocks = reader.GetInt64(reader.GetOrdinal("listed_stocks")),
                                };
                                list.Add(marketCap);
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

        public async Task<List<StockFundamental>> GetStockFundamental(string ticker)
        {
            List<StockFundamental> list = new List<StockFundamental>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"SELECT * FROM stock.stocks_fundamental WHERE ticker = @Ticker ORDER BY trade_date ASC";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Ticker", ticker);
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                StockFundamental fundamental = new StockFundamental
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    trade_date = DateOnly.FromDateTime((DateTime)reader["trade_date"]),
                                    bps = reader.GetInt32(reader.GetOrdinal("bps")),
                                    per = reader.GetDecimal(reader.GetOrdinal("per")),
                                    pbr = reader.GetDecimal(reader.GetOrdinal("pbr")),
                                    eps = reader.GetInt32(reader.GetOrdinal("eps")),
                                    div_yield = reader.GetDecimal(reader.GetOrdinal("div_yield")),
                                    dps = reader.GetInt32(reader.GetOrdinal("dps")),
                                };
                                list.Add(fundamental);
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

        public async Task<List<StockMarketTRX>> GetStockMarketTRX(string ticker)
        {
            List<StockMarketTRX> list = new List<StockMarketTRX>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"SELECT * FROM stock.stocks_market_trx WHERE ticker = @Ticker ORDER BY trade_date ASC";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Ticker", ticker);
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                StockMarketTRX marketTRX = new StockMarketTRX
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    trade_date = DateOnly.FromDateTime((DateTime)reader["trade_date"]),
                                    institution = reader.GetInt64(reader.GetOrdinal("institution")),
                                    corporation = reader.GetInt64(reader.GetOrdinal("corporation")),
                                    individual = reader.GetInt64(reader.GetOrdinal("individual")),
                                    foreigner = reader.GetInt64(reader.GetOrdinal("foreigner")),
                                };
                                list.Add(marketTRX);
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

        public async Task<List<StockSectorTRX>> GetStockSectorTRX(string ticker)
        {
            List<StockSectorTRX> list = new List<StockSectorTRX>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    string query = @"SELECT * FROM stock.stocks_sector_trx WHERE ticker = @Ticker ORDER BY trade_date ASC";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Ticker", ticker);
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                StockSectorTRX sectorTRX = new StockSectorTRX
                                {
                                    ticker = reader.GetString(reader.GetOrdinal("ticker")),
                                    trade_date = DateOnly.FromDateTime((DateTime)reader["trade_date"]),
                                    financial_investment = reader.GetInt64(reader.GetOrdinal("financial_investment")),
                                    insurance = reader.GetInt64(reader.GetOrdinal("insurance")),
                                    investment_trust = reader.GetInt64(reader.GetOrdinal("investment_trust")),
                                    private_equity = reader.GetInt64(reader.GetOrdinal("private_equity")),
                                    bank = reader.GetInt64(reader.GetOrdinal("bank")),
                                    other_financial = reader.GetInt64(reader.GetOrdinal("other_financial")),
                                    pension_fund = reader.GetInt64(reader.GetOrdinal("pension_fund")),
                                    other_corporation = reader.GetInt64(reader.GetOrdinal("other_corporation")),
                                    individual = reader.GetInt64(reader.GetOrdinal("individual")),
                                    foreigner = reader.GetInt64(reader.GetOrdinal("foreigner")),
                                    other_foreigner = reader.GetInt64(reader.GetOrdinal("other_foreigner")),
                                };
                                list.Add(sectorTRX);
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
