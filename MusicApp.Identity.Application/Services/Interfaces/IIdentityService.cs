using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Identity.Application.Services.Interfaces;

public interface IIdentityService
{
    Task<User> Register(UserRegisterDto request);
    Task<string> Login(UserLoginDto request);
    Task<string> RefreshToken(string username);
}
