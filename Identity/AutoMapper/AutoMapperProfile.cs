using AutoMapper;
using Identity.DTOs;
using Identity.Models;

namespace Identity.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserRegisterDto, User>();
        CreateMap<UserLoginDto, User>();
    }
}
