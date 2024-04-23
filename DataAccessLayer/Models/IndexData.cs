namespace DataAccessLayer.Models
{
	public class IndexData
    {
        public string ticker { get; set; } // 티커
        public string name { get; set; } // 이름
        public DateOnly trade_date { get; set; } // 거래일
        public decimal closing_price { get; set; } // 종가
        public long trading_volume { get; set; } // 거래량
        public long market_value { get; set; } // 시가총액
    }
}
