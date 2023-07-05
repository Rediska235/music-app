using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Domain.Exceptions;

namespace MusicApp.PlaylistService.Application.Services.Implementations;

public class PlaylistsService : IPlaylistsService
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISongRepository _songRepository;

    public PlaylistsService(
        IPlaylistRepository playlistRepository,
        IUserRepository userRepository,
        ISongRepository songRepository)
    {
        _playlistRepository = playlistRepository;
        _userRepository = userRepository;
        _songRepository = songRepository;
    }

    public async Task<IEnumerable<Playlist>> GetAllPlaylists(string username)
    {
        var publicPlaylists = await _playlistRepository.GetPublicPlaylistsAsync();
        var myPrivatePlaylists = await _playlistRepository.GetMyPrivatePlaylistsAsync(username);

        return publicPlaylists.Concat(myPrivatePlaylists);
    }

    public async Task<Playlist> GetPlaylistById(Guid id, string username)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);
        if(playlist == null)
        {
            throw CommonExceptions.playlistNotFound;
        }

        if(playlist.IsPrivate && playlist.Creator.Username != username)
        {
            throw CommonExceptions.privatePlaylist;
        }

        return playlist;
    }

    public async Task CreatePlaylist(Playlist playlist, string username)
    {
        var creator = await _userRepository.GetUserByUsername(username);
        playlist.Creator = creator;

        _playlistRepository.CreatePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync();
    }

    public async Task UpdatePlaylist(Playlist playlist, string username)
    {
        var name = playlist.Name;
        var isPrivate = playlist.IsPrivate;

        playlist = await _playlistRepository.GetPlaylistByIdAsync(playlist.Id);
        if (playlist == null)
        {
            throw CommonExceptions.playlistNotFound;
        }

        if (playlist.Creator.Username != username)
        {
            throw CommonExceptions.notYourPlaylist;
        }

        playlist.Name = name;
        playlist.IsPrivate = isPrivate;

        _playlistRepository.UpdatePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync();
    }

    public async Task DeletePlaylist(Guid id, string username)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);
        if (playlist == null)
        {
            throw CommonExceptions.playlistNotFound;
        }

        if (playlist.Creator.Username != username)
        {
            throw CommonExceptions.notYourPlaylist;
        }

        _playlistRepository.DeletePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync();
    }

    public async Task AddSong(Guid playlistId, Guid songId, string username)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
        if (playlist == null)
        {
            throw CommonExceptions.playlistNotFound;
        }

        if (playlist.Creator.Username != username)
        {
            throw CommonExceptions.notYourPlaylist;
        }

        var song = await _songRepository.GetSongByIdAsync(songId);
        if (song == null)
        {
            throw CommonExceptions.songNotFound;
        }

        playlist.Songs.Add(song);

        _playlistRepository.UpdatePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync();
    }

    public async Task RemoveSong(Guid playlistId, Guid songId, string username)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
        if (playlist == null)
        {
            throw CommonExceptions.playlistNotFound;
        }

        if (playlist.Creator.Username != username)
        {
            throw CommonExceptions.notYourPlaylist;
        }

        var song = await _songRepository.GetSongByIdAsync(songId);
        if (song == null)
        {
            throw CommonExceptions.songNotFound;
        }

        playlist.Songs.Remove(song);

        _playlistRepository.UpdatePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync();
    }
}
