﻿namespace BusinessLayer.DTO
{
	public class GetUserDTO
    {
        //[Required(ErrorMessage = "필수로 입력해주세요!")]
        //[StringLength(50,MinimumLength = 3)] // 3~50글자
        public string email { get; set; } // 이메일

        //[DataType(DataType.Password)]
        //[Required]
        public string name { get; set; } // 이름
    }
}
