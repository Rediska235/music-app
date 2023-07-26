using AutoMapper;
using MassTransit;
using MediatR;
using MusicApp.Shared;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services.Interfaces;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateSong;

public class UpdateSongCommandHandler : IRequestHandler<UpdateSongCommand, SongOutputDto>
{
    private readonly ISongRepository _repository;
    private readonly IArtistService _artistService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public UpdateSongCommandHandler(
        ISongRepository repository, 
        IArtistService artistService, 
        IPublishEndpoint publishEndpoint, 
        IMapper mapper)
    {
        _repository = repository;
        _artistService = artistService;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
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

        var songPublishedDto = _mapper.Map<SongPublishedDto>(song);
        var songMessage = new SongMessage()
        {
            Song = songPublishedDto,
            Operation = Operation.Updated
        };
        await _publishEndpoint.Publish(songMessage);

        return _mapper.Map<SongOutputDto>(song);
    }
}