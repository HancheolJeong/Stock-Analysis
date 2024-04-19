using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ETF
    {
        public string ticker { get; set; }
        public string name { get; set; }

        public int closing_price { get; set; }
        public long trading_volume { get; set; }
        public long transaction_amount { get; set; }
        public DateOnly trade_date { get; set; }
    }
}
