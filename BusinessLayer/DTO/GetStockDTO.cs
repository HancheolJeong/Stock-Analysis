using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
    public class GetStockDTO
    {
        public string ticker { get; set; }
        public string? name { get; set; }
        public string? market { get; set; }
    }
}
