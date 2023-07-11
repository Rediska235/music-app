using MediatR;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Application.Repositories;
using FluentValidation;

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

        request.Song.Artist = request.Artist;

        await _repository.CreateSongAsync(request.Song);
        await _repository.SaveChangesAsync();

        return request.Song;
    }
}