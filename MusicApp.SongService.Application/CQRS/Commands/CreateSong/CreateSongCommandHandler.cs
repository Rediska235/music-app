using AutoMapper;
using MassTransit;
using MediatR;
using MusicApp.Shared;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSong;

public class CreateSongCommandHandler : IRequestHandler<CreateSongCommand, SongOutputDto>
{
    private readonly ISongRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateSongCommandHandler(ISongRepository repository, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<SongOutputDto> Handle(CreateSongCommand request, CancellationToken cancellationToken)
    {
        var song = _mapper.Map<Song>(request);

        await _repository.CreateAsync(song, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var songPublishedDto = _mapper.Map<SongPublishedDto>(song);
        await _publishEndpoint.Publish(songPublishedDto);

        return _mapper.Map<SongOutputDto>(song);
    }
}