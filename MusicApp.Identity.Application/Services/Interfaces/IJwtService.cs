using Microsoft.AspNetCore.Http;
using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Identity.Application.Services.Interfaces;

public interface IJwtService
{
    string CreateToken(User user, string secretKey);
    RefreshToken GenerateRefreshToken();
    void SetRefreshToken(RefreshToken refreshToken, HttpContext httpContext, User user);
}
