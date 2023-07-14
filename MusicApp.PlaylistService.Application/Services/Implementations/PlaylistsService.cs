﻿using AutoMapper;
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

    public async Task<IEnumerable<Playlist>> GetPlaylists(CancellationToken cancellationToken)
    {
        var username = _userService.GetUsername();

        var publicPlaylists = await _playlistRepository.GetPublicPlaylistsAsync(cancellationToken);
        var myPrivatePlaylists = await _playlistRepository.GetMyPrivatePlaylistsAsync(username, cancellationToken);

        return publicPlaylists.Concat(myPrivatePlaylists);
    }

    public async Task<Playlist> GetPlaylistById(Guid id, CancellationToken cancellationToken)
    {
        var username = _userService.GetUsername();

        var playlist = await _playlistRepository.GetPlaylistByIdAsync(id, cancellationToken);
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

    public async Task CreatePlaylist(PlaylistInputDto playlistInputDto, CancellationToken cancellationToken)
    {
        await _playlistInputDtoValidator.ValidateAndThrowAsync(playlistInputDto, cancellationToken);

        var user = await _userService.GetOrCreateUser(cancellationToken);

        var playlist = _mapper.Map<Playlist>(playlistInputDto);

        playlist.Creator = user;

        await _playlistRepository.CreatePlaylistAsync(playlist, cancellationToken);
        await _playlistRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePlaylist(Guid id, PlaylistInputDto playlistInputDto, CancellationToken cancellationToken)
    {
        await _playlistInputDtoValidator.ValidateAndThrowAsync(playlistInputDto, cancellationToken);

        var playlist = await _playlistRepository.GetPlaylistByIdAsync(id, cancellationToken);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwner(playlist);

        //Может измениться id (но не должен)
        playlist = _mapper.Map<Playlist>(playlistInputDto);

        _playlistRepository.UpdatePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePlaylist(Guid id, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(id, cancellationToken);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwner(playlist);

        _playlistRepository.DeletePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task AddSong(Guid playlistId, Guid songId, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId, cancellationToken);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwner(playlist);

        var song = await _songRepository.GetSongByIdAsync(songId, cancellationToken);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        playlist.Songs.Add(song);

        _playlistRepository.UpdatePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveSong(Guid playlistId, Guid songId, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId, cancellationToken);
        if (playlist == null)
        {
            throw new PlaylistNotFoundException();
        }

        _userService.ValidateOwner(playlist);

        var song = await _songRepository.GetSongByIdAsync(songId, cancellationToken);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        playlist.Songs.Remove(song);

        _playlistRepository.UpdatePlaylist(playlist);
        await _playlistRepository.SaveChangesAsync(cancellationToken);
    }
}
