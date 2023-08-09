using AutoMapper;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.AutoMapper;

public class SongMapperProfile : Profile
{
    public SongMapperProfile()
    {
        CreateMap<CreateSongCommand, Song>()
            .ForMember(song => song.Title, options => options.MapFrom(src => src.Song.Title));

        CreateMap<Song, SongOutputDto>();

        CreateMap<DelayedSongInputDto, Song>();
    }
}
