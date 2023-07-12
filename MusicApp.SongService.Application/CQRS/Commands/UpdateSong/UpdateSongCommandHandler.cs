using FluentValidation;
using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services;
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

        var title = request.Song.Title;

        var song = await _repository.GetSongByIdAsync(request.Id);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        _artistService.ValidateArtist(song);

        song.Title = title;

        _repository.UpdateSong(song);
        await _repository.SaveChangesAsync();

        return song;
    }
}