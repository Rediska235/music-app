using AutoMapper;
using MediatR;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSong;

public class CreateSongCommandHandler : IRequestHandler<CreateSongCommand, SongOutputDto>
{
    private readonly ISongRepository _repository;
    private readonly IMapper _mapper;

    public CreateSongCommandHandler(ISongRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SongOutputDto> Handle(CreateSongCommand request, CancellationToken cancellationToken)
    {
        var song = _mapper.Map<Song>(request);

        await _repository.CreateAsync(song, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SongOutputDto>(song);
    }
}