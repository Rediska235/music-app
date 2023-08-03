using AutoMapper;
using Grpc.Core;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Web.Grpc.Protos;

namespace MusicApp.PlaylistService.Web.Grpc;

public class GrpcSongService : GrpcSong.GrpcSongBase
{
    private readonly IMapper _mapper;
    private readonly ISongRepository _songRepository;
    private readonly IUserRepository _userRepository;

    public GrpcSongService(IMapper mapper, ISongRepository songRepository, IUserRepository userRepository)
    {
        _mapper = mapper;
        _songRepository = songRepository;
        _userRepository = userRepository;
    }

    public override async Task<Response> SendSongOperation(Request request, ServerCallContext context)
    {
        var song = _mapper.Map<Song>(request.SongOperation.Song);

        var artist = await _userRepository.GetUserByUsernameAsync(song.Artist.Username, context.CancellationToken);
        if(artist != null)
        {
            song.Artist = artist;
        }

        switch (request.SongOperation.Operation)
        {
            case Operation.Added:
                await _songRepository.CreateAsync(song, context.CancellationToken);
                break;
            case Operation.Updated:
                _songRepository.Update(song);
                break;
            case Operation.Removed:
                _songRepository.Delete(song);
                break;
        }

        await _songRepository.SaveChangesAsync(context.CancellationToken);

        return new Response();
    }
}
