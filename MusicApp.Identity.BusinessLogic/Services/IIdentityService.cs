using MusicApp.Identity.BusinessLogic.DTOs;

namespace MusicApp.Identity.BusinessLogic.Services;

public interface IIdentityService
{
    Task<List<string>> Register(UserRegisterDto userRegisterDto);
    Task<string> Login(UserLoginDto userLoginDto);
    Task Logout();
}
