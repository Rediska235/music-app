using MediatR;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Application.Repositories;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSong;

public class CreateArtistCommandHandler : IRequestHandler<CreateSongCommand, Song>
{
    private readonly ISongRepository _repository;
    private readonly CreateSongCommandValidator _validator;

    public CreateArtistCommandHandler(ISongRepository repository)
    {
        _repository = repository;
        _validator = new();
    }

    public async Task<Song> Handle(CreateSongCommand request, CancellationToken cancellationToken)
    {
        _validator.ThrowsExceptionIfRequestIsNotValid(request);

        _repository.CreateSong(request.Song);
        await _repository.SaveChangesAsync();

        return request.Song;
    }
}