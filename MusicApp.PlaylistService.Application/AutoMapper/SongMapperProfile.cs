using AutoMapper;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.Shared;

namespace MusicApp.PlaylistService.Application.AutoMapper;

public class SongMapperProfile : Profile
{
    public SongMapperProfile()
    {
        CreateMap<Song, SongOutputDto>();

        CreateMap<SongPublishedDto, Song>();
    }
}
