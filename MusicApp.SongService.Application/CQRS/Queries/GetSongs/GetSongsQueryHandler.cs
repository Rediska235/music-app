using AutoMapper;
using MediatR;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Application.Repositories;

namespace MusicApp.SongService.Application.CQRS.Queries.GetSongs;

public class GetSongsQueryHandler : IRequestHandler<GetSongsQuery, IEnumerable<SongOutputDto>>
{
    private readonly ISongRepository _repository;
    private readonly IMapper _mapper;

    public GetSongsQueryHandler(ISongRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SongOutputDto>> Handle(GetSongsQuery request, CancellationToken cancellationToken)
    {
        var songs = await _repository.GetAsync(cancellationToken);

        return _mapper.Map<IEnumerable<SongOutputDto>>(songs);
    }
}