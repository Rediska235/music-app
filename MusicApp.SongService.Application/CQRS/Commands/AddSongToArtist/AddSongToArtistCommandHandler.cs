using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateArtist;

public class AddSongToArtistCommandHandler : IRequestHandler<AddSongToArtistCommand, Artist>
{
    private readonly IArtistRepository _repository;

    public AddSongToArtistCommandHandler(IArtistRepository repository)
    {
        _repository = repository;
    }

    public async Task<Artist> Handle(AddSongToArtistCommand request, CancellationToken cancellationToken)
    {
        request.Artist.Songs.Add(request.Song);

        _repository.UpdateArtist(request.Artist);
        await _repository.SaveChangesAsync();

        return request.Artist;
    }
}