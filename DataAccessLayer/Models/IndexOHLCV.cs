using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class IndexOHLCV
    {
        public string ticker {  get; set; } // 티커
        public DateOnly trade_date { get; set; } // 거래일
        public decimal opening_price { get; set; } // 시가
        public decimal high_price { get; set; } // 고가
        public decimal low_price { get; set; } // 저가
        public decimal closing_price { get; set; } // 종가
        public long trading_volume {  get; set; } // 거래량
        public long transaction_amount { get; set; } // 거래대금
        public long market_value { get; set; } // 시가총액
    }
}
