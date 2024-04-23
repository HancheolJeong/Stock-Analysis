namespace BusinessLayer.DTO
{
	public class GetStockFundamentalDTO
    {
        public string ticker { get; set; } // 티커
        public DateOnly trade_date { get; set; } // 거래일
        public int bps { get; set; } // Book-value Per Share 주당장부가치
        public decimal per { get; set; } // Price Earning ratio 주가수익비율
        public decimal pbr { get; set; } // Price to Book-value Ratio 주가순자산비율
        public int eps { get; set; } // Earnings Per Share 주당순이익
        public decimal div_yield { get; set; } // Dividend Yield 배당수익률
        public int dps { get; set; } // Dividends Per Share 주당배당금
    }
}
