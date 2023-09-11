using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Tests.Identity.IntegrationTests;

public class TestData
{
    public static User User = new()
    {
        Id = new Guid("40543b82-9493-40f7-a2c8-04f41f844bb7"),
        Username = "Anton",
        RefreshToken = "refreshToken"
    };

    public static UserRegisterDto UserForRegister = new()
    {
        Username = "Dmitry",
        Password = "12345678"
    };

    public static UserLoginDto UserForLogin = new()
    {
        Username = "Nikolay",
        Password = "12345678"
    };

    public static UserLoginDto UserForLogin2 = new()
    {
        Username = "Martin",
        Password = "Password"
    };

    public static UserLoginDto UserForLogin3 = new()
    {
        Username = "Martin",
        Password = "WrongPassword"
    };
      
    public static UserLoginDto UserWithIncorrectCreds = new()
    {
        Username = "a",
        Password = "123"
    };
}