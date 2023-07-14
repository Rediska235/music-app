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

    public PlaylistsService(
        IPlaylistRepository playlistRepository,
        ISongRepository songRepository,
        UserService userService)
    {
        _playlistRepository = playlistRepository;
        _songRepository = songRepository;
        _userService = userService;
    }

    public async Task<IEnumerable<Playlist>> GetAllPlaylists()
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

    public async Task CreatePlaylist(Playlist playlist)
    {
        var user = await _userService.GetOrCreateUser();

        playlist.Creator = user;

        await _playlistRepository.CreatePlaylistAsync(playlist);
        await _playlistRepository.SaveChangesAsync();
    }

    public async Task UpdatePlaylist(Playlist playlist)
    {
        var name = playlist.Name;
        var isPrivate = playlist.IsPrivate;

        playlist = await _playlistRepository.GetPlaylistByIdAsync(playlist.Id);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwner(playlist);

        playlist.Name = name;
        playlist.IsPrivate = isPrivate;

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
