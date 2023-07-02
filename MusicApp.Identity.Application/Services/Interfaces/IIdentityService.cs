using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Identity.Application.Services.Interfaces;

public interface IIdentityService
{
    User Register(UserRegisterDto request);
    string Login(UserLoginDto request, string secretKey);
    string RefreshToken(string username, string secretKey);
}
