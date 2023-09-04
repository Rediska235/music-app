using AutoMapper;
using MediatR;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services.Interfaces;

namespace MusicApp.SongService.Application.CQRS.Queries.GetFavoriteSongs;

public class GetFavoriteSongsQueryHandler : IRequestHandler<GetFavoriteSongsQuery, IEnumerable<SongOutputDto>>
{
    private readonly ISongMongoDbRepository _mongoRepository;
    private readonly ISongRepository _repository;
    private readonly IMapper _mapper;
    private readonly IArtistService _artistService;

    public GetFavoriteSongsQueryHandler(ISongMongoDbRepository mongoRepository, 
        IMapper mapper, 
        IArtistService artistService,
        ISongRepository repository)
    {
        _mongoRepository = mongoRepository;
        _mapper = mapper;
        _artistService = artistService;
        _repository = repository;
    }

    public async Task<IEnumerable<SongOutputDto>> Handle(GetFavoriteSongsQuery request, CancellationToken cancellationToken)
    {
        var username = _artistService.GetUsername();
        var songIds = await _mongoRepository.GetFavoriteSongs(username);

        var favoriteSongs = new List<SongOutputDto>();
        foreach(var songId in songIds)
        {
            var song = await _repository.GetByIdAsync(songId, cancellationToken);
            favoriteSongs.Add(_mapper.Map<SongOutputDto>(song));
        }

        return favoriteSongs;
    }
}