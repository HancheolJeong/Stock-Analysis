using BusinessLayer.DTO;

namespace BusinessLayer.Services
{
    public interface ILoginService
    {
        Task CreateUser(CreateUserDTO createUserDTO);
        Task<List<GetAllUserResponseDTO>> GetAllUser();
        Task<GetUserResponseDTO> GetUser(GetUserDTO getUserDTO);
    }
}