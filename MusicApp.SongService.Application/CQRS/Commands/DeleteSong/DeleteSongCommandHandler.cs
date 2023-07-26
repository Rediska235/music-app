using AutoMapper;
using MassTransit;
using MediatR;
using MusicApp.Shared;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services.Interfaces;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.CQRS.Commands.DeleteSong;

public class DeleteSongCommandHandler : IRequestHandler<DeleteSongCommand>
{
    private readonly ISongRepository _repository;
    private readonly IArtistService _artistService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public DeleteSongCommandHandler(ISongRepository repository, IArtistService artistService, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _repository = repository;
        _artistService = artistService;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    public async Task Handle(DeleteSongCommand request, CancellationToken cancellationToken)
    {
        var song = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (song == null)
        {
            throw new SongNotFoundException();
        }

        _artistService.ValidateArtistAndThrow(song);

        _repository.Delete(song);
        await _repository.SaveChangesAsync(cancellationToken);

        var songPublishedDto = _mapper.Map<SongPublishedDto>(song);
        var songMessage = new SongMessage()
        {
            Song = songPublishedDto,
            Operation = Operation.Deleted
        };
        await _publishEndpoint.Publish(songMessage);
    }
}