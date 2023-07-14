using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.CQRS.Commands.DeleteSong;

public class DeleteSongCommandHandler : IRequestHandler<DeleteSongCommand, Song>
{
    private readonly ISongRepository _repository;
    private readonly ArtistService _artistService;

    public DeleteSongCommandHandler(ISongRepository repository, ArtistService artistService)
    {
        _repository = repository;
        _artistService = artistService;
    }

    public async Task<Song> Handle(DeleteSongCommand request, CancellationToken cancellationToken)
    {
        var song = await _repository.GetSongByIdAsync(request.Id, cancellationToken);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        _artistService.ValidateArtistAndThrow(song);

        _repository.DeleteSong(song);
        await _repository.SaveChangesAsync(cancellationToken);

        return song;
    }
}