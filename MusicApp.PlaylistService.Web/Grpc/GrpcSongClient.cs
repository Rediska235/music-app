using AutoMapper;
using Grpc.Net.Client;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Web.Grpc.Protos;

namespace MusicApp.PlaylistService.Web.Grpc;

public class GrpcSongClient
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public GrpcSongClient(IMapper mapper, IConfiguration configuration)
    {
        _mapper = mapper;
        _configuration = configuration;
    }

    public IEnumerable<Song> GetSongsFromSongService()
    {
        var channel = GrpcChannel.ForAddress(_configuration["GrpcHost"]);
        
        var client = new GrpcSong.GrpcSongClient(channel);
        var request = new GetSongsRequest();

        var response = client.GetSongs(request);

        return _mapper.Map<IEnumerable<Song>>(response.Songs);
    }
}
