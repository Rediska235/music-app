using AutoMapper;
using MusicApp.Identity.BusinessLogic.DTOs;
using MusicApp.Identity.DataAccess.Models;

namespace MusicApp.Identity.BusinessLogic.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserRegisterDto, User>();
        CreateMap<UserLoginDto, User>();
    }
}
