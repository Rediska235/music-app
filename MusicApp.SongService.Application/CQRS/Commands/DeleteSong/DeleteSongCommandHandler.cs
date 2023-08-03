using MediatR;
using MusicApp.SongService.Application.Grpc;
using MusicApp.SongService.Application.Grpc.Protos;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.CQRS.Commands.DeleteSong;

public class DeleteSongCommandHandler : IRequestHandler<DeleteSongCommand>
{
    private readonly ISongRepository _repository;
    private readonly ArtistService _artistService;
    private readonly GrpcSongClient _client;

    public DeleteSongCommandHandler(ISongRepository repository, ArtistService artistService, GrpcSongClient client)
    {
        _repository = repository;
        _artistService = artistService;
        _client = client;
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

        _client.SendSongOperation(song, Operation.Removed);
    }
}