using AutoMapper;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Application.Grpc.Protos;

namespace MusicApp.SongService.Application.Grpc;

public class GrpcSongClient
{
    private readonly IMapper _mapper;
    private readonly GrpcSong.GrpcSongClient _client;

    public GrpcSongClient(IMapper mapper, GrpcSong.GrpcSongClient client)
    {
        _mapper = mapper;
        _client = client;
    }

    public virtual void SendSongOperation(Song song, Operation operation)
    {
        var songMessage = _mapper.Map<GrpcSongModel>(song);

        var songOperation = new GrpcSongOperationModel();
        songOperation.Song = songMessage;
        songOperation.Operation = operation;

        var request = new Request();
        request.SongOperation = songOperation;

        _client.SendSongOperation(request);
    }
}
