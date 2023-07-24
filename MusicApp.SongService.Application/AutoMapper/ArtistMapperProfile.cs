using AutoMapper;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.AutoMapper;

public class ArtistMapperProfile : Profile
{
    public ArtistMapperProfile()
    {
        CreateMap<Artist, ArtistOutputDto>();
    }
}
