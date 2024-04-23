namespace DataAccessLayer.Models
{
	public class StockMarketTRX
    {
        public string ticker { get; set; } // 티커
        public DateOnly trade_date { get; set; } // 거래일
        public long institution {  get; set; } // 기관
        public long corporation { get; set; } // 법인
        public long individual { get; set; } // 개인
        public long foreigner { get; set; } // 외국인

    }
}
