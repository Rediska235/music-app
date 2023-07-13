using FluentValidation;
using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSong;

public class CreateSongCommandHandler : IRequestHandler<CreateSongCommand, Song>
{
    private readonly ISongRepository _repository;
    private readonly CreateSongCommandValidator _validator;
    
    public CreateSongCommandHandler(ISongRepository repository)
    {
        _repository = repository;
        _validator = new();
    }

    public async Task<Song> Handle(CreateSongCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request);

        var song = new Song()
        {
            Id = Guid.NewGuid(),
            Title = request.Song.Title,
            Artist = request.Artist
        };

        await _repository.CreateSongAsync(song);
        await _repository.SaveChangesAsync();

        return song;
    }
}