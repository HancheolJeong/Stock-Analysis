using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using System.Data;

namespace BusinessLayer.Services
{
    public class LoginService : ILoginService
    {
        IProcCall _procCall;

        /// <summary>
        /// Users 테이블과 관련된 비즈니스 로직 인스턴스를 초기화 합니다.
        /// </summary>
        /// <param name="procCall">데이터베이스 프로시저 처리 객체 주입</param>
        public LoginService(IProcCall procCall)
        {
            this._procCall = procCall;
        }

        /// <summary>
        /// 로그인 기록을 추가하거나 업데이트 하는 비즈니스로직 논리값을 리턴한다.
        /// </summary>
        /// <param name="getUserDTO"></param>
        /// <returns>처리결과</returns>
        public async Task<(bool, string?)> Login(GetUserDTO getUserDTO)
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => { });
                Mapper mapper = new Mapper(configuration);
                Dictionary<string, object> dc = mapper.Map<GetUserDTO, Dictionary<string, object>>(getUserDTO); // 프로시저 변수로 사용하기 위해서 dto를 dictionary로 변환한다.
                DataTable dt = await _procCall.RequestProcedure("UpsertUserByEmail", dc);

                bool result = dt.Rows[0]["Result"].ToString() == "fail" ? false : true; // "fail" 이라는 결과를 받았다면 false를 리턴하고 아니면 true를 리턴한다.
                return (result, null);
            }
            catch (Exception ex) 
            {
                string err = $"예외 발생 : BusinessLayer/Services/LoginService/(Function)Login {ex.Message}";
                return (false, err);
            }

        }

    }
}
