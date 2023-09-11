using AutoMapper;
using MediatR;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Application.Grpc;
using MusicApp.SongService.Application.Grpc.Protos;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services.Interfaces;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateSong;

public class UpdateSongCommandHandler : IRequestHandler<UpdateSongCommand, SongOutputDto>
{
    private readonly ISongRepository _repository;
    private readonly IArtistService _artistService;
    private readonly IMapper _mapper;
    private readonly GrpcSongClient _client;

    public UpdateSongCommandHandler(
        ISongRepository repository, 
        IArtistService artistService, 
        IMapper mapper, 
        GrpcSongClient client)
    {
        _repository = repository;
        _artistService = artistService;
        _mapper = mapper;
        _client = client;
    }

    public async Task<SongOutputDto> Handle(UpdateSongCommand request, CancellationToken cancellationToken)
    {
        var song = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        _artistService.ValidateArtistAndThrow(song);

        song.Title = request.Song.Title;

        _repository.Update(song);
        await _repository.SaveChangesAsync(cancellationToken);

        _client.SendSongOperation(song, Operation.Updated);

        return _mapper.Map<SongOutputDto>(song);
    }
}