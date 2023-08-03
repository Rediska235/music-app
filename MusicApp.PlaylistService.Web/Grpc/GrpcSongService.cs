using AutoMapper;
using Grpc.Core;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Web.Grpc.Protos;
using System.Text.Json;

namespace MusicApp.PlaylistService.Web.Grpc;

public class GrpcSongService : GrpcSong.GrpcSongBase
{
    private readonly IMapper _mapper;
    private readonly ISongRepository _songRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GrpcSongService> _logger;

    public GrpcSongService(IMapper mapper, ISongRepository songRepository, ILogger<GrpcSongService> logger, IUserRepository userRepository)
    {
        _mapper = mapper;
        _songRepository = songRepository;
        _logger = logger;
        _userRepository = userRepository;
    }

    public override async Task<Response> SendSongOperation(Request request, ServerCallContext context)
    {
        var song = _mapper.Map<Song>(request.SongOperation.Song);

        _logger.LogCritical(JsonSerializer.Serialize(song));

        switch (request.SongOperation.Operation)
        {
            case Operation.Added:
                await _songRepository.CreateAsync(song, context.CancellationToken);
                break;
            case Operation.Updated:
                _songRepository.Update(song);
                break;
            case Operation.Removed:
                _logger.LogCritical("Delete");
                _songRepository.Delete(song);                                    //в логах ничего не показывается
                _logger.LogCritical("Get");
                await _songRepository.GetAsync(context.CancellationToken);       //в логах есть гет запрос
                break;
        }
        _logger.LogCritical("return");

        return new Response();
    }
}
