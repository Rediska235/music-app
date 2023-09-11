using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Application.Services.Implementations;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Domain.Exceptions;
using MusicApp.Shared;
using System.Security.Claims;

namespace MusicApp.Tests.PlaylistService.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
    private readonly Mock<HttpContext> _httpContextMock = new();
    private readonly Mock<ClaimsPrincipal> _claimsPrincipal = new();
    private readonly Mock<ClaimsIdentity> _claimIdentity = new();
    private readonly string _username = "username";
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<UserService>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly UserService _service;

    public UserServiceTests()
    {
        _httpContextAccessorMock.Setup(_httpContextAccessorMock =>
            _httpContextAccessorMock.HttpContext)
                .Returns(_httpContextMock.Object);
        _httpContextMock.Setup(_httpContextMock => _httpContextMock.User).Returns(_claimsPrincipal.Object);
        _claimsPrincipal.Setup(user => user.Identity).Returns(_claimIdentity.Object);
        _claimIdentity.Setup(identity => identity.Name).Returns(_username);

        _service = new UserService(
            _httpContextAccessorMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetUserAsync_WhenUserNotFound_ShouldThrowException()
    {
        // Arrange

        // Act
        var act = async () => await _service.GetUserAsync(_cancellationToken);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnUser()
    {
        // Arrange
        var user = _fixture.Create<User>();

        _userRepositoryMock.Setup(_userRepositoryMock =>
            _userRepositoryMock.GetUserByUsernameAsync(It.IsAny<string>(), _cancellationToken))
                .ReturnsAsync(user);

        // Act
        var result = await _service.GetUserAsync(_cancellationToken);

        // Assert
        result.Should().Be(user);
    }

    [Fact]
    public async Task AddUserAsync_ShouldReturnUser()
    {
        // Arrange
        var userPublishedDto = _fixture.Create<UserPublishedDto>();
        var user = _fixture.Create<User>();

        _mapperMock.Setup(_mapperMock => _mapperMock.Map<User>(userPublishedDto)).Returns(user);

        // Act
        var result = await _service.AddUserAsync(userPublishedDto, _cancellationToken);

        // Assert
        result.Should().Be(user);
    }

    [Fact]
    public void ValidateOwnerAndThrow_WhenNotYourPlaylist_ShouldThrowException()
    {
        // Arrange
        var playlist = _fixture.Create<Playlist>();

        // Act
        var act = () => _service.ValidateOwnerAndThrow(playlist);

        // Assert
        act.Should().Throw<NotYourPlaylistException>();
    }

    [Fact]
    public void ValidateOwnerAndThrow_ShouldReturnWithoutExceptions()
    {
        // Arrange
        var playlist = _fixture.Create<Playlist>();
        playlist.Creator.Username = _username;

        // Act
        var act = () => _service.ValidateOwnerAndThrow(playlist);

        // Assert
        act.Should().NotThrow<Exception>();
    }

    [Fact]
    public void GetUsername_ShouldReturnUsername()
    {
        // Arrange

        // Act
        var result = _service.GetUsername();

        // Assert
        result.Should().Be(_username);
    }
}