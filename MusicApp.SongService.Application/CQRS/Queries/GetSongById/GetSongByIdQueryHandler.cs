using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

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
        return await _repository.GetSongByIdAsync(request.Id);
    }
}