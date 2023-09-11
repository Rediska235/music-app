using AutoMapper;
using MediatR;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Application.Grpc;
using MusicApp.SongService.Application.Grpc.Protos;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSong;

public class CreateSongCommandHandler : IRequestHandler<CreateSongCommand, SongOutputDto>
{
    private readonly ISongRepository _repository;
    private readonly IMapper _mapper;
    private readonly GrpcSongClient _client;

    public CreateSongCommandHandler(ISongRepository repository, IMapper mapper, GrpcSongClient client)
    {
        _repository = repository;
        _mapper = mapper;
        _client = client;
    }

    public async Task<SongOutputDto> Handle(CreateSongCommand request, CancellationToken cancellationToken)
    {
        var song = _mapper.Map<Song>(request);

        await _repository.CreateAsync(song, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _client.SendSongOperation(song, Operation.Added);

        return _mapper.Map<SongOutputDto>(song);
    }
}