using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class AdvancedStock
    {
        public string? ticker { get; set; }
        public string? name { get; set; }
        public string? market { get; set; }
        public int closing_price { get; set; }
        public long market_value { get; set; }
        public long trading_volume { get; set; }
        public long listed_stocks { get; set; }
        public long transaction_amount { get; set; } //거래대금
        public DateOnly trade_date { get; set; }
    }
}
