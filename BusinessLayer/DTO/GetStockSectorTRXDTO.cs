using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
    public class GetStockSectorTRXDTO
    {
        public string ticker { get; set; } // 티커
        public DateOnly trade_date { get; set; } // 거래일
        public long financial_investment { get; set; } // 금융투자
        public long insurance { get; set; } // 보험
        public long investment_trust { get; set; } // 투신
        public long private_equity { get; set; } // 사모펀드
        public long bank { get; set; } // 은행
        public long other_financial { get; set; } // 기타금융
        public long pension_fund { get; set; } // 연기금
        public long other_corporation { get; set; } // 기타법인
        public long individual { get; set; } // 개인
        public long foreigner { get; set; } // 외국인
        public long other_foreigner { get; set; } // 기타외국인
    }
}
