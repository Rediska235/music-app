using AutoMapper;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Web.Grpc.Protos;

namespace MusicApp.SongService.Web.Automapper;

public class GrpcModelsMapperProfile : Profile
{
    public GrpcModelsMapperProfile()
    {
        CreateMap<Song, GrpcSongModel>()
            .ForMember(song => song.Id, options => options.MapFrom(src => src.Id.ToString()));

        CreateMap<Artist, GrpcArtistModel>()
            .ForMember(artist => artist.Id, options => options.MapFrom(src => src.Id.ToString()));
    }
}
