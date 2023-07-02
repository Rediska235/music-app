using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Identity.Application.Services.Interfaces;
using MusicApp.Identity.Application.DTOs;

namespace MusicApp.Identity.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly string _secretKey;

    public IdentityController(IIdentityService identityService, IConfiguration configuration)
    {
        _identityService = identityService;
        _secretKey = configuration.GetSection("JWT:Key").Value;
    }

    [HttpPost("register")]
    public IActionResult Register(UserRegisterDto request)
    {
        var user = _identityService.Register(request);

        return Ok(user);
    }

    [HttpPost("login")]
    public IActionResult Login(UserLoginDto request)
    {
        var token = _identityService.Login(request, _secretKey);

        return Ok(token);
    }

    [HttpGet("refresh-token"), Authorize(AuthenticationSchemes = "ExpiredTokenAllowed")]
    public IActionResult RefreshToken()
    {
        var username = User.Identity.Name;
        var refreshToken = _identityService.RefreshToken(username, _secretKey);

        return Ok(refreshToken);
    }
}