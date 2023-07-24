using Microsoft.AspNetCore.Mvc;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Application.Services.Interfaces;

namespace MusicApp.Identity.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto request)
    {
        var user = await _identityService.Register(request);

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto request)
    {
        var token = await _identityService.Login(request);

        return Ok(token);
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var username = User.Identity.Name;
        var refreshToken = await _identityService.RefreshToken(username);

        return Ok(refreshToken);
    }
}