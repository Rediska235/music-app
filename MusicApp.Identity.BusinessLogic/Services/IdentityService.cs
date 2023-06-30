using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MusicApp.Identity.BusinessLogic.DTOs;
using MusicApp.Identity.DataAccess.Models;

namespace MusicApp.Identity.BusinessLogic.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;

    public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<List<string>> Register(UserRegisterDto userRegisterDto)
    {
        var user = _mapper.Map<User>(userRegisterDto);
        var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
        if (!result.Succeeded)
        {
            var errors = new List<string>();
            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
            }

            return errors;
        }

        if (user.IsArtist)
        {
            await _userManager.AddToRoleAsync(user, "artist");
        }

        await _signInManager.SignInAsync(user, false);

        return Enumerable.Empty<string>().ToList();
    }

    public async Task<string> Login(UserLoginDto userLoginDto)
    {
        var result = await _signInManager.PasswordSignInAsync(userLoginDto.UserName, userLoginDto.Password, false, false);

        return result.Succeeded ? string.Empty : "Неверный логин или пароль";
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
}
