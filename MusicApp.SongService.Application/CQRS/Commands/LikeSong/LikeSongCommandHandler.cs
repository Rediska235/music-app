using MediatR;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services.Interfaces;

namespace MusicApp.SongService.Application.CQRS.Commands.LikeSong;

public class LikeSongCommandHandler : IRequestHandler<LikeSongCommand>
{
    private readonly ISongMongoDbRepository _mongoRepository;
    private readonly IArtistService _artistService;

    public LikeSongCommandHandler(ISongMongoDbRepository mongoRepository, IArtistService artistService)
    {
        _mongoRepository = mongoRepository;
        _artistService = artistService;
    }

    public async Task Handle(LikeSongCommand request, CancellationToken cancellationToken)
    {
        var username = _artistService.GetUsername();

        var favoriteSongs = await _mongoRepository.GetFavoriteSongsAsync(username);
        if (favoriteSongs == null)
        {
            await _mongoRepository.AddUser(username);
        }
        
        await _mongoRepository.LikeSongAsync(request.Id, username);
    }
}