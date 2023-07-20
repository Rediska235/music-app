using AutoMapper;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Domain.Exceptions;

namespace MusicApp.PlaylistService.Application.Services.Implementations;

public class PlaylistsService : IPlaylistsService
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly ISongRepository _songRepository;
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public PlaylistsService(
        IPlaylistRepository playlistRepository,
        ISongRepository songRepository,
        UserService userService,
        IMapper mapper)
    {
        _playlistRepository = playlistRepository;
        _songRepository = songRepository;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PlaylistOutputDto>> GetPlaylistsAsync(CancellationToken cancellationToken)
    {
        var username = _userService.GetUsername();

        var publicPlaylists = await _playlistRepository.GetPublicPlaylistsAsync(cancellationToken);
        var myPrivatePlaylists = await _playlistRepository.GetMyPrivatePlaylistsAsync(username, cancellationToken);

        var playlists = publicPlaylists.Concat(myPrivatePlaylists);

        return _mapper.Map<IEnumerable<PlaylistOutputDto>>(playlists);
    }

    public async Task<PlaylistOutputDto> GetPlaylistByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var username = _userService.GetUsername();

        var playlist = await _playlistRepository.GetByIdAsync(id, cancellationToken);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        if (playlist.IsPrivate && playlist.Creator.Username != username)
        {
            throw new PrivatePlaylistException();
        }

        return _mapper.Map<PlaylistOutputDto>(playlist);
    }

    public async Task<PlaylistOutputDto> CreatePlaylistAsync(PlaylistInputDto playlistInputDto, CancellationToken cancellationToken)
    {
        var playlist = _mapper.Map<Playlist>(playlistInputDto);

        var user = await _userService.GetOrCreateUser(cancellationToken);
        playlist.Creator = user;

        await _playlistRepository.CreateAsync(playlist, cancellationToken);
        await _playlistRepository.SaveChangesAsync(cancellationToken);

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
    }
}
