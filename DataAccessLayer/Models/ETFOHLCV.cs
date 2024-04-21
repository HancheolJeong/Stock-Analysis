using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ETFOHLCV
    {
        public string ticker {  get; set; } // 티커
        public DateOnly trade_date { get; set; } // 거래일
        public decimal nav {  get; set; } // 순자산가치
        public int opening_price { get; set; } // 시가
        public int high_price { get; set; } // 고가
        public int low_price { get; set; } // 저가
        public int closing_price { get; set; } // 종가
        public long trading_volume { get; set; } // 거래량
        public long transaction_amount { get; set; } // 거래대금
        public decimal basic_index { get; set; }// 기초지수


    }
}
