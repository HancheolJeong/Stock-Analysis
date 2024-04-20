﻿using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappers
{
    public class ETFMapper : IETFMapper
    {
        private string connectionString;
        public ETFMapper(string conn)
        {
            connectionString = conn;
        }

        public async Task<List<ETF>> GetETFData()
        {
            List<ETF> list = new List<ETF>();
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
        stock.etfs_ohlcv
    GROUP BY 
        ticker
)
SELECT 
    i.ticker, 
    i.name, 
    o.closing_price, 
    o.trading_volume, 
    o.transaction_amount, 
    o.trade_date
FROM 
    stock.etfs i
JOIN 
    stock.etfs_ohlcv o ON i.ticker = o.ticker
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
    }
}