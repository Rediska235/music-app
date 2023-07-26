using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services.Interfaces;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.CQRS.Queries.GetArtist;

public class GetArtistQueryHandler : IRequestHandler<GetArtistQuery, Artist>
{
    private readonly IArtistRepository _repository;
    private readonly IArtistService _service;

    public GetArtistQueryHandler(IArtistRepository repository, IArtistService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<Artist> Handle(GetArtistQuery request, CancellationToken cancellationToken)
    {
        var username = _service.GetUsername();
        var artist = await _repository.GetArtistByUsernameAsync(username, cancellationToken);
        if (artist == null)
        {
            throw new ArtistNotFoundException();
        }

        return artist;
    }
}