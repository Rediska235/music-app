using AutoMapper;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.Shared;

namespace MusicApp.PlaylistService.Application.AutoMapper;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserOutputDto>();

        CreateMap<UserPublishedDto, User>();
    }
}
