using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Identity.Application.Services.Interfaces;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Web.Filters;

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
    [RegisterValidationFilter]
    public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
    {
        var user = await _identityService.Register(userRegisterDto);

        return Ok(user);
    }

    [HttpPost("login")]
    [LoginValidationFilter]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        var token = await _identityService.Login(userLoginDto);

        return Ok(token);
    }

    [HttpGet("refresh-token"), Authorize(AuthenticationSchemes = "ExpiredTokenAllowed")]
    public async Task<IActionResult> RefreshToken()
    {
        var username = User.Identity.Name;
        var refreshToken = await _identityService.RefreshToken(username);

        return Ok(refreshToken);
    }
}