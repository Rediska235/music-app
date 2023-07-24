using AutoMapper;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.AutoMapper;

public class PlaylistMapperProfile : Profile
{
    public PlaylistMapperProfile()
    {
        CreateMap<PlaylistInputDto, Playlist>();

        CreateMap<Playlist, PlaylistOutputDto>();
    }
}
