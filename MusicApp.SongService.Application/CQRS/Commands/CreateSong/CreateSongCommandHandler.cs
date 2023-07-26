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
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public CreateSongCommandHandler(ISongRepository repository, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    public async Task<SongOutputDto> Handle(CreateSongCommand request, CancellationToken cancellationToken)
    {
        var song = _mapper.Map<Song>(request);

        await _repository.CreateAsync(song, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var songPublishedDto = _mapper.Map<SongPublishedDto>(song);
        var songMessage = new SongMessage()
        {
            Song = songPublishedDto,
            Operation = Operation.Created
        };
        await _publishEndpoint.Publish(songMessage);

        return _mapper.Map<SongOutputDto>(song);
    }
}