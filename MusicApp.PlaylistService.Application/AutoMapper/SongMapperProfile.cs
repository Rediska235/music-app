using AutoMapper;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.AutoMapper;

public class SongMapperProfile : Profile
{
    public SongMapperProfile()
    {
        CreateMap<Song, SongOutputDto>();
    }
}
