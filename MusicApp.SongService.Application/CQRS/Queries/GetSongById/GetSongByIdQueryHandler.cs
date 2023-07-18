﻿using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.CQRS.Queries.GetSongById;

public class GetSongByIdQueryHandler : IRequestHandler<GetSongByIdQuery, Song>
{
    private readonly ISongRepository _repository;

    public GetSongByIdQueryHandler(ISongRepository repository)
    {
        _repository = repository;
    }

    public async Task<Song> Handle(GetSongByIdQuery request, CancellationToken cancellationToken)
    {
        var song = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        return song;
    }
}