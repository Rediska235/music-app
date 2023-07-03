using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateSong;

public class UpdateSongCommandHandler : IRequestHandler<UpdateSongCommand, Song>
{
    private readonly ISongRepository _repository;
    private readonly UpdateSongCommandValidator _validator;

    public UpdateSongCommandHandler(ISongRepository repository)
    {
        _repository = repository;
        _validator = new();
    }

    public async Task<Song> Handle(UpdateSongCommand request, CancellationToken cancellationToken)
    {
        _validator.ThrowsExceptionIfRequestIsNotValid(request);

        _repository.UpdateSong(request.Song);
        await _repository.SaveChangesAsync();

        return request.Song;
    }
}