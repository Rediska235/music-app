using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Queries.GetArtistByUsername;

public class GetArtistByUsernameQueryHandler : IRequestHandler<GetArtistByUsernameQuery, Artist>
{
    private readonly IArtistRepository _repository;

    public GetArtistByUsernameQueryHandler(IArtistRepository repository)
    {
        _repository = repository;
    }

    public async Task<Artist> Handle(GetArtistByUsernameQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetArtistByUsername(request.Username);
    }
}