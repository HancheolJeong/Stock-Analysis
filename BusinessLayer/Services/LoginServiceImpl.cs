using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using System.Data;

namespace BusinessLayer.Services
{
    public class LoginServiceImpl : ILoginService
    {
        IProcCall _procCall;

        /// <summary>
        /// Users 테이블과 관련된 비즈니스 로직 인스턴스를 초기화 합니다.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="procCall"></param>
        public LoginServiceImpl(IProcCall procCall)
        {
            this._procCall = procCall;
        }

        /// <summary>
        /// 로그인 기록을 추가하거나 업데이트 하는 비즈니스로직 논리값을 리턴한다.
        /// </summary>
        /// <param name="getUserDTO"></param>
        /// <returns></returns>
        public async Task<bool> Login(GetUserDTO getUserDTO)
        {
            var configuration = new MapperConfiguration(cfg => { });
            Mapper mapper = new Mapper(configuration);
            Dictionary<string, object> dc = mapper.Map<GetUserDTO, Dictionary<string, object>>(getUserDTO); // 프로시저 변수로 사용하기 위해서 dto를 dictionary로 변환한다.
            DataTable dt = await _procCall.RequestProcedure("UpsertUserByEmail", dc);

            bool result = dt.Rows[0]["Result"].ToString() == "fail" ? false : true;
            return result;
        }

    }
}
