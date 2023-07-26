using AutoMapper;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Shared;

namespace MusicApp.Identity.Application.AutoMapper;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserRegisterDto, User>()
            .ForMember(user => user.PasswordHash, options => options.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));

        CreateMap<User, UserPublishedDto>();
    }
}
