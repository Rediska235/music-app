using MediatR;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Application.Repositories;

namespace MusicApp.SongService.Application.CQRS.Commands.DeleteSong;

public class DeleteSongCommandHandler : IRequestHandler<DeleteSongCommand, Song>
{
    private readonly ISongRepository _repository;

    public DeleteSongCommandHandler(ISongRepository repository)
    {
        _repository = repository;
    }

    public async Task<Song> Handle(DeleteSongCommand request, CancellationToken cancellationToken)
    {
        _repository.DeleteSong(request.Song);
        await _repository.SaveChangesAsync();

        return request.Song;
    }
}