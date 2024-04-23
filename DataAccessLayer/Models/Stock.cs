using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Stock
    {
        public string? ticker { get; set; } // 티커
        public string? name { get; set; } // 종목명
        public string? market { get; set; } // 시장
        public int closing_price { get; set; } // 종가
        public long market_value { get; set; } // 시가총액
        public long trading_volume { get; set; } // 거래량
        public long listed_stocks { get; set; } // 상장주식수
        public long transaction_amount { get; set; } // 거래대금
        public DateOnly trade_date { get; set; } // 거래일
    }
}
