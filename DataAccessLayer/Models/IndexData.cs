using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class IndexData
    {
        public string ticker { get; set; }
        public string name { get; set; }
        public DateOnly trade_date { get; set; }
        public decimal closing_price { get; set; }
        public long trading_volume { get; set; }
        public long market_value { get; set; }
    }
}
