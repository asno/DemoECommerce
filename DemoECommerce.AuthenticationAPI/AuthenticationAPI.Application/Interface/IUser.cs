using AuthenticationAPI.Application.DTO;
using ECommerce.SharedLibrary.Responses;

namespace AuthenticationAPI.Application.Interface
{
    public interface IUser
    {
        Task<Response> Register(AppUserDTO appUserDTO);
        Task<Response> Login(LoginDTO loginDTO);
        Task<AppUserDTO> GetUser(int userId);
    }
}
