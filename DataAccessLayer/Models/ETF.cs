namespace DataAccessLayer.Models
{
	public class ETF
    {
        public string ticker { get; set; } // 티커
        public string name { get; set; } // 이름

        public int closing_price { get; set; } // 종가
        public long trading_volume { get; set; } // 거래량
        public long transaction_amount { get; set; } // 거래대금
        public DateOnly trade_date { get; set; } // 거래일
    }
}
