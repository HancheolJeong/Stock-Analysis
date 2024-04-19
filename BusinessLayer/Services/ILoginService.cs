using BusinessLayer.DTO;
using DataAccessLayer.Models;

namespace BusinessLayer.Services
{
    public interface ILoginService
    {
        Task<bool> Login(GetUserDTO getUserDTO);
    }
}