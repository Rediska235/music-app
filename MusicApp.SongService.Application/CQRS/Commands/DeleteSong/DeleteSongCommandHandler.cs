using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services.Interfaces;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.CQRS.Commands.DeleteSong;

public class DeleteSongCommandHandler : IRequestHandler<DeleteSongCommand>
{
    private readonly ISongRepository _repository;
    private readonly IArtistService _artistService;

    public DeleteSongCommandHandler(ISongRepository repository, IArtistService artistService)
    {
        _repository = repository;
        _artistService = artistService;
    }

    public async Task Handle(DeleteSongCommand request, CancellationToken cancellationToken)
    {
        var song = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        _artistService.ValidateArtistAndThrow(song);

        _repository.Delete(song);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}