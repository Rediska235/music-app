using AutoMapper;
using MusicApp.Shared;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.AutoMapper;

public class ArtistMapperProfile : Profile
{
    public ArtistMapperProfile()
    {
        CreateMap<Artist, ArtistOutputDto>();

        CreateMap<UserPublishedDto, Artist>();

        CreateMap<Artist, UserPublishedDto>();
    }
}
