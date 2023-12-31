﻿using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Application.Repositories;
using MusicApp.Identity.Application.Services.Interfaces;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Identity.Domain.Exceptions;
using MusicApp.Shared;
using System.Text.Json;

namespace MusicApp.Identity.Application.Services.Implementations;

public class IdentityService : IIdentityService
{
    private readonly HttpContext _httpContext;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtManager;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IConfiguration configuration,
        IJwtService jwtManager,
        IMapper mapper,
        IPublishEndpoint publishEndpoint,
        ILogger<IdentityService> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _configuration = configuration;
        _jwtManager = jwtManager;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _logger = logger;

        _httpContext = httpContextAccessor.HttpContext;
    }

    public async Task<User> Register(UserRegisterDto request)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username);
        if(user != null)
        {
            throw new UsernameIsTakenException();
        }

        user = _mapper.Map<User>(request);

        var artistRole = await _roleRepository.GetRoleByTitleAsync("artist");
        if (user.IsArtist)
        {
            user.Roles.Add(artistRole);
        }

        await _userRepository.InsertUserAsync(user);
        await _userRepository.SaveChangesAsync();

        var userPublishedDto = _mapper.Map<UserPublishedDto>(user);
        await _publishEndpoint.Publish(userPublishedDto);
        
        _logger.LogInformation($"Register{{\"userRegisterDto\": {JsonSerializer.Serialize(request)}}}");

        return user;
    }

    public async Task<string> Login(UserLoginDto request)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username);
        if(user == null)
        {
            throw new InvalidUsernameOrPasswordException();
        }

        var isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if(!isValidPassword)
        {
            throw new InvalidUsernameOrPasswordException();
        }

        var secretKey = _configuration.GetSection("JWT:Key").Value;
        var token = _jwtManager.CreateToken(user, secretKey);
        var refreshToken = _jwtManager.GenerateRefreshToken();
        _jwtManager.SetRefreshToken(refreshToken, _httpContext, user);

        await _userRepository.SaveChangesAsync();

        _logger.LogInformation($"Login{{\"userLoginDto\": {JsonSerializer.Serialize(request)}}}");

        return token;
    }

    public async Task<string> RefreshToken(string username)
    {
        var refreshToken = _httpContext.Request.Cookies["refreshToken"];

        var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
        if(user == null || user.Username != username)
        {
            throw new InvalidRefreshTokenException();
        }

        var secretKey = _configuration.GetSection("JWT:Key").Value;
        var token = _jwtManager.CreateToken(user, secretKey);
        var newRefreshToken = _jwtManager.GenerateRefreshToken();
        _jwtManager.SetRefreshToken(newRefreshToken, _httpContext, user);

        await _userRepository.SaveChangesAsync();

        _logger.LogInformation($"RefreshToken{{\"username\": {username}");

        return token;
    }
}
