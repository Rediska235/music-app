using AutoMapper;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Web.Grpc.Protos;

namespace MusicApp.PlaylistService.Web.Grpc;

public class GrpcSongClient
{
    private readonly IMapper _mapper;
    private readonly GrpcSong.GrpcSongClient _client;

    public GrpcSongClient(IMapper mapper, GrpcSong.GrpcSongClient client)
    {
        _mapper = mapper;
        _client = client;
    }

    public IEnumerable<Song> GetSongsFromSongService()
    {
        var request = new GetSongsRequest();

        var response = _client.GetSongs(request);

        return _mapper.Map<IEnumerable<Song>>(response.Songs);
    }
}
