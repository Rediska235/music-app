using FluentValidation;
using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateSong;

public class UpdateSongCommandHandler : IRequestHandler<UpdateSongCommand, Song>
{
    private readonly ISongRepository _repository;
    private readonly UpdateSongCommandValidator _validator;
    private readonly ArtistService _artistService;

    public UpdateSongCommandHandler(ISongRepository repository, ArtistService artistService)
    {
        _repository = repository;
        _artistService = artistService;
        _validator = new();
    }

    public async Task<Song> Handle(UpdateSongCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request);

        var song = request.Song;
        var title = song.Title;

        song = await _repository.GetSongByIdAsync(request.Song.Id);
        if (song == null)
        {
            throw CommonExceptions.songNotFound;
        }

        _artistService.ValidateArtist(song);

        song.Title = title;

        _repository.UpdateSong(request.Song);
        await _repository.SaveChangesAsync();

        return request.Song;
    }
}