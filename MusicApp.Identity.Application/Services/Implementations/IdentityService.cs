using AutoMapper;
using Microsoft.AspNetCore.Http;
using MusicApp.Identity.Application.Repositories;
using MusicApp.Identity.Application.Services.Interfaces;
using MusicApp.Identity.Application.Validators;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Identity.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using FluentValidation;

namespace MusicApp.Identity.Application.Services.Implementations;

public class IdentityService : IIdentityService
{
    private readonly HttpContext _httpContext;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly UserRegisterDtoValidator _userRegisterDtoValidator;
    private readonly UserLoginDtoValidator _userLoginDtoValidator;
    private readonly IConfiguration _configuration;
    private readonly JwtManager _jwtManager;

    public IdentityService(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IConfiguration configuration,
        JwtManager jwtManager,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _configuration = configuration;
        _jwtManager = jwtManager;
        _mapper = mapper;

        _httpContext = httpContextAccessor.HttpContext;

        _userRegisterDtoValidator = new UserRegisterDtoValidator();
        _userLoginDtoValidator = new UserLoginDtoValidator();
    }

    public async Task<User> Register(UserRegisterDto request)
    {
        await _userRegisterDtoValidator.ValidateAndThrowAsync(request);

        var user = await _userRepository.GetUserByUsernameAsync(request.Username);
        if(user != null)
        {
            throw CommonExceptions.usernameIsTaken;
        }

        user = _mapper.Map<User>(request);

        var artistRole = await _roleRepository.GetRoleByTitleAsync("artist");
        if (user.IsArtist)
        {
            user.Roles.Add(artistRole);
        }

        await _userRepository.InsertUserAsync(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }

    public async Task<string> Login(UserLoginDto request)
    {
        await _userLoginDtoValidator.ValidateAndThrowAsync(request);

        var user = await _userRepository.GetUserByUsernameAsync(request.Username);
        if(user == null)
        {
            throw CommonExceptions.invalidCredential;
        }

        var isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if(!isValidPassword)
        {
            throw CommonExceptions.invalidCredential;
        }

        var secretKey = _configuration.GetSection("JWT:Key").Value;
        var token = _jwtManager.CreateToken(user, secretKey);
        var refreshToken = _jwtManager.GenerateRefreshToken();
        _jwtManager.SetRefreshToken(refreshToken, _httpContext, user);

        await _userRepository.SaveChangesAsync();

        return token;
    }

    public async Task<string> RefreshToken(string username)
    {
        var refreshToken = _httpContext.Request.Cookies["refreshToken"];

        var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
        if(user == null || user.Username != username)
        {
            throw CommonExceptions.invalidRefreshToken;
        }

        var secretKey = _configuration.GetSection("JWT:Key").Value;
        var token = _jwtManager.CreateToken(user, secretKey);
        var newRefreshToken = _jwtManager.GenerateRefreshToken();
        _jwtManager.SetRefreshToken(newRefreshToken, _httpContext, user);

        await _userRepository.SaveChangesAsync();

        return token;
    }
}
