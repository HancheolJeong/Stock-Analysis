using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
    public class GetUserResponseDTO
    {
        //id, userid, username, point
        public int Id { get; set; }
        public string? Userid { get; set; }
        public string? Username { get; set; }
        public int Point { get; set; }
    }
}
