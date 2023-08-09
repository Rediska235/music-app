using AutoMapper;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Web.Grpc.Protos;

namespace MusicApp.PlaylistService.Web.AutoMapper;

public class GrpcModelsMapperProfile : Profile
{
    public GrpcModelsMapperProfile()
    {
        CreateMap<GrpcSongModel, Song>()
            .ForMember(song => song.Id, options => options.MapFrom(src => new Guid(src.Id)));

        CreateMap<GrpcArtistModel, User>()
            .ForMember(artist => artist.Id, options => options.MapFrom(src => new Guid(src.Id)));
    }
}
