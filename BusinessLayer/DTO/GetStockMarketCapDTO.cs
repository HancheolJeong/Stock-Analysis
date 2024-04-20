using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
    public class GetStockMarketCapDTO
    {
        public string ticker { get; set; } // 티커
        public DateOnly trade_date { get; set; } // 거래일
        public long market_value { get; set; } // 시가총액
        public long trading_volume { get; set; } // 거래량
        public long transaction_amount { get; set; } // 거래대금
        public long listed_stocks { get; set; } // 상장주식수
    }
}
