using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
	public interface ILoginService
    {
        Task<(bool, string?)> Login(GetUserDTO getUserDTO);  //사용자를 추가하거나 업데이트하는 함수 처리결과를 반환한다 
    }
}