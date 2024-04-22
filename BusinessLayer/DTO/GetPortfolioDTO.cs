using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
    public class GetPortfolioDTO
    {
        public int? id { get; set; } // id
        public string ticker { get; set; } // 티커 
        public string market { get; set; } // 시장
        public int amount { get; set; } // 양
        public int unit_price { get; set; } // 단가
        public DateOnly? create_dt { get; set; } // 생성일
        public int TotalValue { get; set; } // amount * unit_price

        public string? name { get; set; } // 종목명

        public int? current_price { get; set; } // 현재가
    }
}
