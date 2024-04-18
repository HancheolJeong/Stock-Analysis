using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface ILoginService
    {
        Task<bool> Login(GetUserDTO getUserDTO);
    }
}