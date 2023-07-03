using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Queries.GetAllSongs;

public class GetAllSongsQueryHandler : IRequestHandler<GetAllSongsQuery, IEnumerable<Song>>
{
    private readonly ISongRepository _repository;

    public GetAllSongsQueryHandler(ISongRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Song>> Handle(GetAllSongsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllSongsAsync();
    }
}