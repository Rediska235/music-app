using AutoMapper;
using Grpc.Core;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Web.Grpc.Protos;

namespace MusicApp.SongService.Web.Grpc;

public class GrpcSongService : GrpcSong.GrpcSongBase
{
    private readonly IMapper _mapper;
    private readonly ISongRepository _repository;

    public GrpcSongService(IMapper mapper, ISongRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public override async Task<SongResponse> GetSongs(GetSongsRequest request, ServerCallContext context)
    {
        var response = new SongResponse();
        var songs = await _repository.GetAsync(new CancellationToken());

        foreach (var song in songs)
        {
            response.Songs.Add(_mapper.Map<GrpcSongModel>(song));
        }

        return response;
    }
}
