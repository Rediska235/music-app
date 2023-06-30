using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MusicApp.Identity.BusinessLogic.DTOs;
using MusicApp.Identity.BusinessLogic.Services;

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

    //  обработать ошибки

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
    {
        var errors = await _identityService.Register(userRegisterDto);

        if(!errors.IsNullOrEmpty())
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        await _identityService.Login(userLoginDto);

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _identityService.Logout();

        return Ok();
    }
}