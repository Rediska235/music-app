using MediatR;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Application.Repositories;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateArtist;

public class CreateArtistCommandHandler : IRequestHandler<CreateArtistCommand, Artist>
{
    private readonly IArtistRepository _repository;

    public CreateArtistCommandHandler(IArtistRepository repository)
    {
        _repository = repository;
    }

    public async Task<Artist> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
    {
        var artist = new Artist()
        {
            Username = request.Username
        };

        _repository.CreateArtist(artist);
        await _repository.SaveChangesAsync();

        return artist;
    }
}