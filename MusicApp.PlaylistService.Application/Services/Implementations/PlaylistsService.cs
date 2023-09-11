using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Domain.Exceptions;
using System.Text.Json;

namespace MusicApp.PlaylistService.Application.Services.Implementations;

public class PlaylistsService : IPlaylistsService
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly ISongRepository _songRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlaylistsService> _logger;

    public PlaylistsService(
        IPlaylistRepository playlistRepository,
        ISongRepository songRepository,
        IUserService userService,
        IMapper mapper,
        ILogger<PlaylistsService> logger)
    {
        _playlistRepository = playlistRepository;
        _songRepository = songRepository;
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PlaylistOutputDto>> GetPlaylistsAsync(CancellationToken cancellationToken)
    {
        var username = _userService.GetUsername();

        var publicPlaylists = await _playlistRepository.GetPublicPlaylistsAsync(cancellationToken);
        var myPrivatePlaylists = await _playlistRepository.GetMyPrivatePlaylistsAsync(username, cancellationToken);

        var playlists = publicPlaylists.Concat(myPrivatePlaylists);

        _logger.LogInformation("GetPlaylistsAsync{}");

        return _mapper.Map<IEnumerable<PlaylistOutputDto>>(playlists);
    }

    public async Task<PlaylistOutputDto> GetPlaylistByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetByIdAsync(id, cancellationToken);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        var username = _userService.GetUsername();
        if (playlist.IsPrivate && playlist.Creator.Username != username)
        {
            throw new PrivatePlaylistException();
        }

        _logger.LogInformation($"GetPlaylistByIdAsync{{\"id\": {id}}}");

        return _mapper.Map<PlaylistOutputDto>(playlist);
    }

    public async Task<PlaylistOutputDto> CreatePlaylistAsync(PlaylistInputDto playlistInputDto, CancellationToken cancellationToken)
    {
        var playlist = _mapper.Map<Playlist>(playlistInputDto);

        var user = await _userService.GetUserAsync(cancellationToken);
        playlist.Creator = user;

        await _playlistRepository.CreateAsync(playlist, cancellationToken);
        await _playlistRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"CreatePlaylistAsync{{\"playlistInputDto\": {JsonSerializer.Serialize(playlistInputDto)}}}");

        return _mapper.Map<PlaylistOutputDto>(playlist);
    }

    public async Task<PlaylistOutputDto> UpdatePlaylistAsync(Guid id, PlaylistInputDto playlistInputDto, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetByIdAsync(id, cancellationToken);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwnerAndThrow(playlist);

        _mapper.Map(playlistInputDto, playlist);

        _playlistRepository.Update(playlist);
        await _playlistRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"UpdatePlaylistAsync{{\"id\": {id}, \"playlistInputDto\": {JsonSerializer.Serialize(playlistInputDto)}}}");

        return _mapper.Map<PlaylistOutputDto>(playlist);
    }

    public async Task DeletePlaylistAsync(Guid id, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetByIdAsync(id, cancellationToken);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwnerAndThrow(playlist);

        _playlistRepository.Delete(playlist);
        await _playlistRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"DeletePlaylistAsync{{\"id\": {id}}}");
    }

    public async Task AddSongAsync(Guid playlistId, Guid songId, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetByIdAsync(playlistId, cancellationToken);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwnerAndThrow(playlist);

        var song = await _songRepository.GetByIdAsync(songId, cancellationToken);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        playlist.Songs.Add(song);

        _playlistRepository.Update(playlist);
        await _playlistRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"AddSongAsync{{\"playlistId\": {playlistId}, \"songId\": {songId}}}");
    }

    public async Task RemoveSongAsync(Guid playlistId, Guid songId, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetByIdAsync(playlistId, cancellationToken);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwnerAndThrow(playlist);

        var song = await _songRepository.GetByIdAsync(songId, cancellationToken);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        playlist.Songs.Remove(song);

        _playlistRepository.Update(playlist);
        await _playlistRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"RemoveSongAsync{{\"playlistId\": {playlistId}, \"songId\": {songId}}}");
    }
}
