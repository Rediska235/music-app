using AutoMapper;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Identity.Application.AutoMapper;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserRegisterDto, User>()
            .ForMember(u => u.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
    }
}
