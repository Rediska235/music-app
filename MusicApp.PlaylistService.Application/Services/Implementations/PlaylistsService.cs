using AutoMapper;
using FluentValidation;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Application.Validators;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Domain.Exceptions;

namespace MusicApp.PlaylistService.Application.Services.Implementations;

public class PlaylistsService : IPlaylistsService
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly ISongRepository _songRepository;
    private readonly UserService _userService;
    private readonly PlaylistInputDtoValidator _playlistInputDtoValidator;
    private readonly IMapper _mapper;

    public PlaylistsService(
        IPlaylistRepository playlistRepository,
        ISongRepository songRepository,
        UserService userService,
        PlaylistInputDtoValidator playlistInputDtoValidator,
        IMapper mapper)
    {
        _playlistRepository = playlistRepository;
        _songRepository = songRepository;
        _userService = userService;
        _playlistInputDtoValidator = playlistInputDtoValidator;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Playlist>> GetPlaylists()
    {
        var username = _userService.GetUsername();

        var publicPlaylists = await _playlistRepository.GetPublicPlaylistsAsync();
        var myPrivatePlaylists = await _playlistRepository.GetMyPrivatePlaylistsAsync(username);

        return publicPlaylists.Concat(myPrivatePlaylists);
    }

    public async Task<Playlist> GetPlaylistById(Guid id)
    {
        var username = _userService.GetUsername();

        var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        if (playlist.IsPrivate && playlist.Creator.Username != username)
        {
            throw new PrivatePlaylistException();
        }

        return playlist;
    }

    public async Task CreatePlaylist(PlaylistInputDto playlistInputDto)
    {
        await _playlistInputDtoValidator.ValidateAndThrowAsync(playlistInputDto);

        var user = await _userService.GetOrCreateUser();

        var playlist = _mapper.Map<Playlist>(playlistInputDto);

        playlist.Creator = user;

        await _playlistRepository.CreatePlaylistAsync(playlist);
        await _playlistRepository.SaveChangesAsync();
    }

    public async Task UpdatePlaylist(Guid id, PlaylistInputDto playlistInputDto)
    {
        await _playlistInputDtoValidator.ValidateAndThrowAsync(playlistInputDto);

        var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwner(playlist);

        //Может измениться id (но не должен)
        playlist = _mapper.Map<Playlist>(playlistInputDto);

        _playlistRepository.UpdatePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync();
    }

    public async Task DeletePlaylist(Guid id)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwner(playlist);

        _playlistRepository.DeletePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync();
    }

    public async Task AddSong(Guid playlistId, Guid songId)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwner(playlist);

        var song = await _songRepository.GetSongByIdAsync(songId);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        playlist.Songs.Add(song);

        _playlistRepository.UpdatePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync();
    }

    public async Task RemoveSong(Guid playlistId, Guid songId)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwner(playlist);

        var song = await _songRepository.GetSongByIdAsync(songId);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        playlist.Songs.Remove(song);

        _playlistRepository.UpdatePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync();
    }
}
