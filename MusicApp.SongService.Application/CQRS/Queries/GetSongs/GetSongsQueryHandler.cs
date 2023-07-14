using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Queries.GetSongs;

public class GetSongsQueryHandler : IRequestHandler<GetSongsQuery, IEnumerable<Song>>
{
    private readonly ISongRepository _repository;

    public GetSongsQueryHandler(ISongRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Song>> Handle(GetSongsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetSongsAsync();
    }
}