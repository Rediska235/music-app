using AutoMapper;
using Microsoft.AspNetCore.Http;
using MusicApp.Identity.Application.Repositories;
using MusicApp.Identity.Application.Services.Interfaces;
using MusicApp.Identity.Application.Validators;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Identity.Domain.Exceptions;
using FluentValidation.Results;

namespace MusicApp.Identity.Application.Services.Implementations;

public class IdentityService : IIdentityService
{
    private readonly HttpContext _httpContext;
    private readonly IUserRepository _userRepository;
    private readonly Role _artistRole;
    private readonly IMapper _mapper;
    private readonly UserRegisterDtoValidator _userRegisterDtoValidator;
    private readonly UserLoginDtoValidator _userLoginDtoValidator;

    public IdentityService(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;

        _httpContext = httpContextAccessor.HttpContext;
        _artistRole = roleRepository.GetRoleByTitle("artist");

        _userRegisterDtoValidator = new UserRegisterDtoValidator();
        _userLoginDtoValidator = new UserLoginDtoValidator();
    }

    public User Register(UserRegisterDto request)
    {
        var result = _userRegisterDtoValidator.Validate(request);
        ThrowsExceptionIfRequestIsNotValid(result);

        var user = _userRepository.GetUserByUsername(request.Username);
        if(user != null)
        {
            throw CommonExceptions.usernameIsTaken;
        }

        user = _mapper.Map<User>(request);

        if(user.IsArtist)
        {
            user.Roles.Add(_artistRole);
        }

        _userRepository.InsertUser(user);
        _userRepository.Save();

        return user;
    }

    public string Login(UserLoginDto request, string secretKey)
    {
        var result = _userLoginDtoValidator.Validate(request);
        ThrowsExceptionIfRequestIsNotValid(result);

        var user = _userRepository.GetUserByUsername(request.Username);
        if(user == null)
        {
            throw CommonExceptions.invalidCredential;
        }

        var isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if(!isValidPassword)
        {
            throw CommonExceptions.invalidCredential;
        }

        var token = JwtManager.CreateToken(user, secretKey);
        var refreshToken = JwtManager.GenerateRefreshToken();
        JwtManager.SetRefreshToken(refreshToken, _httpContext, user);

        _userRepository.Save();

        return token;
    }

    public string RefreshToken(string username, string secretKey)
    {
        var refreshToken = _httpContext.Request.Cookies["refreshToken"];

        var user = _userRepository.GetUserByRefreshToken(refreshToken);
        if(user == null || user.Username != username)
        {
            throw CommonExceptions.invalidRefreshToken;
        }

        var token = JwtManager.CreateToken(user, secretKey);
        var newRefreshToken = JwtManager.GenerateRefreshToken();
        JwtManager.SetRefreshToken(newRefreshToken, _httpContext, user);

        _userRepository.Save();

        return token;
    }

    private void ThrowsExceptionIfRequestIsNotValid(ValidationResult result)
    {
        if (!result.IsValid)
        {
            var errorList = "";
            foreach (var failure in result.Errors)
            {
                errorList += failure.ErrorMessage + ';';
            }
            errorList = errorList.Remove(errorList.Length - 1);

            throw new ArgumentException(errorList);
        }
    }
}
