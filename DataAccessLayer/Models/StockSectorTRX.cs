using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class StockSectorTRX
    {
        public string ticker { get; set; }
        public DateOnly trade_date { get; set; }
        public int financial_investment { get; set; }
        public int insurance { get; set; }
        public int investment_trust { get; set; }
        public int private_equity { get; set; }
        public int bank { get; set; }
        public int other_financial { get; set; }
        public int pension_fund { get; set; }
        public int other_corporation { get; set; }
        public int individual { get; set; }
        public int foreigner { get; set; }
        public int other_foreigner { get; set; }
    }
}
