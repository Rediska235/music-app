using Microsoft.AspNetCore.Mvc;
using MusicApp.Identity.BusinessLogic.DTOs;
using MusicApp.Identity.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MusicApp.Identity.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IConfiguration _configuration;

    public IdentityController(IIdentityService identityService, IConfiguration configuration)
    {
        _identityService = identityService;
        _configuration = configuration;
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
        string secretKey = _configuration.GetSection("JWT:Key").Value;
        var token = _identityService.Login(request, secretKey);

        return Ok(token);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        _identityService.Logout();
        return Ok();
    }

    [HttpGet("refresh-token"), Authorize(AuthenticationSchemes = "ExpiredTokenAllowed")]
    public IActionResult RefreshToken()
    {
        var username = User.Identity.Name;
        var secretKey = _configuration.GetSection("JWT:Key").Value;
        var refreshToken = _identityService.RefreshToken(username, secretKey);

        return Ok(refreshToken);
    }
}