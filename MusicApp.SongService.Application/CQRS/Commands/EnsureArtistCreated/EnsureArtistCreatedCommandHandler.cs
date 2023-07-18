using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.EnsureArtistCreated;

public class EnsureArtistCreatedCommandHandler : IRequestHandler<EnsureArtistCreatedCommand, Artist>
{
    private readonly IArtistRepository _repository;
    private readonly ArtistService _artistService;

    public EnsureArtistCreatedCommandHandler(IArtistRepository repository, ArtistService artistService)
    {
        _repository = repository;
        _artistService = artistService;
    }

    public async Task<Artist> Handle(EnsureArtistCreatedCommand request, CancellationToken cancellationToken)
    {
        var username = _artistService.GetUsername();
        var artist = await _repository.GetArtistByUsernameAsync(username, cancellationToken);
        if(artist != null)
        {
            return artist;
        }

        artist = new Artist()
        {
            Id = Guid.NewGuid(),
            Username = username
        };

        await _repository.CreateAsync(artist, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return artist;
    }
}