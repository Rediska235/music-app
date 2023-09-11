using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Domain.Exceptions;
using MusicApp.Shared;
using System.Text.Json;

namespace MusicApp.PlaylistService.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository repository, IMapper mapper, ILogger<UserService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<User> GetUserAsync(CancellationToken cancellationToken)
    {
        var username = GetUsername();

        var user = await _repository.GetUserByUsernameAsync(username, cancellationToken);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        _logger.LogInformation("GetUserAsync{}");

        return user;
    }

    public async Task<User> AddUserAsync(UserPublishedDto userPublishedDto, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(userPublishedDto);

        await _repository.CreateAsync(user, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"AddUserAsync{{\"userPublishedDto\": {JsonSerializer.Serialize(userPublishedDto)}}}");

        return user;
    }

    public void ValidateOwnerAndThrow(Playlist playlist)
    {
        var username = GetUsername();

        if (playlist.Creator.Username != username)
        {
            throw new NotYourPlaylistException();
        }
    }

    public string GetUsername()
    {
        return _httpContextAccessor.HttpContext.User.Identity.Name;
    }
}
