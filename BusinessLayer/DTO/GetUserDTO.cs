using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
    public class GetUserDTO
    {
        //[Required(ErrorMessage = "필수로 입력해주세요!")]
        //[StringLength(50,MinimumLength = 3)] // 3~50글자
        public string email { get; set; }

        //[DataType(DataType.Password)]
        //[Required]
        public string name { get; set; }
    }
}
