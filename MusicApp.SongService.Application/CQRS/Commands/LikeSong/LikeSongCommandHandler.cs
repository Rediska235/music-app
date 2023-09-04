using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services.Interfaces;

namespace MusicApp.SongService.Application.CQRS.Commands.LikeSong;

public class LikeSongCommandHandler : IRequestHandler<LikeSongCommand>
{
    private readonly ISongMongoDbRepository _repository;
    private readonly IArtistService _artistService;

    public LikeSongCommandHandler(ISongMongoDbRepository repository, IArtistService artistService)
    {
        _repository = repository;
        _artistService = artistService;
    }

    public async Task Handle(LikeSongCommand request, CancellationToken cancellationToken)
    {
        var username = _artistService.GetUsername();
        
        await _repository.LikeSong(request.Id, username);
    }
}