using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Application.Repositories;
using MusicApp.Identity.Application.Services.Implementations;
using MusicApp.Identity.Application.Services.Interfaces;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Identity.Domain.Exceptions;

namespace MusicApp.Tests.Identity.UnitTests.Services;

public class IdentityServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IRoleRepository> _roleRepositoryMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
    private readonly Mock<ILogger<IdentityService>> _loggerMock = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
    private readonly Mock<HttpContext> _httpContextMock = new();
    private readonly Fixture _fixture = new();
    private readonly IdentityService _identityService;

    public IdentityServiceTests()
    {
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(_httpContextMock.Object);

        _identityService = new IdentityService(
            _httpContextAccessorMock.Object,
            _userRepositoryMock.Object,
            _roleRepositoryMock.Object,
            _configurationMock.Object,
            _jwtServiceMock.Object,
            _mapperMock.Object,
            _publishEndpointMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async void Register_WhenUsernameIsTaken_ShouldThrowException()
    {
        //Arrange
        _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(new User());

        //Act
        var act = async () => await _identityService.Register(new UserRegisterDto());

        //Assert
        await act.Should().ThrowAsync<UsernameIsTakenException>();
    }

    [Fact]
    public async void Register_WhenUserIsNotArtist_ShouldReturnUserWithoutArtistRole()
    {
        //Arrange
        _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User)null);
        _mapperMock.Setup(x => x.Map<User>(It.IsAny<UserRegisterDto>())).Returns(new User());
        _roleRepositoryMock.Setup(x => x.GetRoleByTitleAsync(It.IsAny<string>())).ReturnsAsync(new Role());
        _userRepositoryMock.Setup(x => x.InsertUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

        //Act
        var result = await _identityService.Register(new UserRegisterDto());

        //Assert
        result.Should().BeOfType<User>();
        result.Roles.Should().BeEmpty();
    }

    [Fact]
    public async void Register_WhenUserIsArtist_ShouldReturnUserWithArtistRole()
    {
        //Arrange
        var user = new User
        {
            IsArtist = true
        };

        var role = new Role
        {
            Title = "artist"
        };
        
        _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User)null);
        _mapperMock.Setup(x => x.Map<User>(It.IsAny<UserRegisterDto>())).Returns(user);
        _roleRepositoryMock.Setup(x => x.GetRoleByTitleAsync(It.IsAny<string>())).ReturnsAsync(role);
        _userRepositoryMock.Setup(x => x.InsertUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

        //Act
        var result = await _identityService.Register(new UserRegisterDto());

        //Assert
        result.Should().BeOfType<User>();
        result.Roles.Should().Contain(role => role.Title == "artist");
    }

    [Fact]
    public async void Login_WhenUserNotExists_ShouldThrowException()
    {
        //Arrange
        _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User)null);

        //Act
        var act = async () => await _identityService.Login(new UserLoginDto());

        //Assert
        await act.Should().ThrowAsync<InvalidUsernameOrPasswordException>();
    }

    [Fact]
    public async void Login_WhenIncorrectPassword_ShouldThrowException()
    {
        //Arrange
        var password = "password";
        var wrongPassword = "wrongPassword";

        var user = new User
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        var userLoginDto = new UserLoginDto
        {
            Password = wrongPassword
        };

        _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Act
        var act = async () => await _identityService.Login(userLoginDto);

        //Assert
        await act.Should().ThrowAsync<InvalidUsernameOrPasswordException>();
    }

    [Fact]
    public async void Login_WhenAllGood_ShouldReturnToken()
    {
        //Arrange
        var password = _fixture.Create<string>();
        var token = _fixture.Create<string>();

        var user = new User
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        var userLoginDto = new UserLoginDto
        {
            Password = password
        };

        _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value).Returns(string.Empty);
        _jwtServiceMock.Setup(x => x.CreateToken(It.IsAny<User>(), It.IsAny<string>())).Returns(token);
        _jwtServiceMock.Setup(x => x.GenerateRefreshToken()).Returns(new RefreshToken());
        _userRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

        //Act
        var result = await _identityService.Login(userLoginDto);

        //Assert
        result.Should().Be(token);
    }

    [Fact]
    public async void RefreshToken_WhenUserNotExists_ShouldThrowException()
    {
        //Arrange
        var refreshToken = _fixture.Create<string>();

        _httpContextMock.Setup(x => x.Request.Cookies[It.IsAny<string>()]).Returns(refreshToken);
        _userRepositoryMock.Setup(x => x.GetUserByRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync((User)null);

        //Act
        var act = async () => await _identityService.RefreshToken(string.Empty);

        //Assert
        await act.Should().ThrowAsync<InvalidRefreshTokenException>();
    }

    [Fact]
    public async void RefreshToken_WhenNotYourToken_ShouldThrowException()
    {
        //Arrange
        var refreshToken = _fixture.Create<string>();
        var username = "username";
        var otherUserUsername = "otherUserUsername";

        var otherUser = new User
        {
            Username = otherUserUsername
        };

        _httpContextMock.Setup(x => x.Request.Cookies[It.IsAny<string>()]).Returns(refreshToken);
        _userRepositoryMock.Setup(x => x.GetUserByRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync(otherUser);

        //Act
        var act = async () => await _identityService.RefreshToken(username);

        //Assert
        await act.Should().ThrowAsync<InvalidRefreshTokenException>();
    }

    [Fact]
    public async void RefreshToken_WhenAllGood_ShouldReturnToken()
    {
        //Arrange
        var refreshToken = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var token = _fixture.Create<string>();

        var user = new User
        {
            Username = username
        };

        _httpContextMock.Setup(x => x.Request.Cookies[It.IsAny<string>()]).Returns(refreshToken);
        _userRepositoryMock.Setup(x => x.GetUserByRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync(user);
        _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value).Returns(string.Empty);
        _jwtServiceMock.Setup(x => x.CreateToken(It.IsAny<User>(), It.IsAny<string>())).Returns(token);
        _jwtServiceMock.Setup(x => x.GenerateRefreshToken()).Returns(new RefreshToken());
        _userRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

        //Act
        var result = await _identityService.RefreshToken(username);

        //Assert
        result.Should().Be(token);
    }
}
