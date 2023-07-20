using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateSong;

public class UpdateSongCommandHandler : IRequestHandler<UpdateSongCommand, Song>
{
    private readonly ISongRepository _repository;
    private readonly ArtistService _artistService;

    public UpdateSongCommandHandler(ISongRepository repository, ArtistService artistService)
    {
        _repository = repository;
        _artistService = artistService;
    }

    public async Task<Song> Handle(UpdateSongCommand request, CancellationToken cancellationToken)
    {
        var song = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        _artistService.ValidateArtistAndThrow(song);

        song.Title = request.Song.Title;

        _repository.Update(song);
        await _repository.SaveChangesAsync(cancellationToken);

        return song;
    }
}