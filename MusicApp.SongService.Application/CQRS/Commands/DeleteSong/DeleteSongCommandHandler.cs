﻿using MediatR;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Application.Repositories;

namespace MusicApp.SongService.Application.CQRS.Commands.DeleteSong;

public class DeleteSongCommandHandler : IRequestHandler<DeleteSongCommand, Song>
{
    private readonly ISongRepository _repository;
    private readonly ArtistService _ownerCheckService;

    public DeleteSongCommandHandler(ISongRepository repository, ArtistService ownerCheckService)
    {
        _repository = repository;
        _ownerCheckService = ownerCheckService;
    }

    public async Task<Song> Handle(DeleteSongCommand request, CancellationToken cancellationToken)
    {
        var song = await _repository.GetSongByIdAsync(request.Id);

        _ownerCheckService.ValidateArtist(song);

        _repository.DeleteSong(song);
        await _repository.SaveChangesAsync();

        return song;
    }
}