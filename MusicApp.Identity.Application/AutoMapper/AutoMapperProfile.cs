using AutoMapper;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Identity.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserRegisterDto, User>()
            .ForMember("PasswordHash", opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
    }
}
