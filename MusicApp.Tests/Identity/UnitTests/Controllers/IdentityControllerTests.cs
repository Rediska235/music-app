using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Application.Services.Interfaces;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Identity.Web.Controllers;
using System.Security.Claims;

namespace MusicApp.Tests.Identity.UnitTests.Controllers;

public class IdentityControllerTests
{
    private readonly Mock<IIdentityService> _identityServiceMock = new();
    private readonly Fixture _fixture = new();
    private readonly IdentityController _controller;

    public IdentityControllerTests()
    {
        _controller = new IdentityController(_identityServiceMock.Object);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Register_ReturnsOkObjectResultWithUser()
    {
        // Arrange
        var userRegisterDto = _fixture.Create<UserRegisterDto>();
        var user = _fixture.Create<User>();
        var task = Task.FromResult(user);
        
        _identityServiceMock.Setup(x => x.Register(It.IsAny<UserRegisterDto>())).Returns(task);

        // Act
        var result = await _controller.Register(userRegisterDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(user);
    }

    [Fact]
    public async Task Login_ReturnsOkObjectResultWithToken()
    {
        // Arrange
        var userLoginDto = _fixture.Create<UserLoginDto>();
        var token = _fixture.Create<string>();
        var task = Task.FromResult(token);

        _identityServiceMock.Setup(x => x.Login(It.IsAny<UserLoginDto>())).Returns(task);

        // Act
        var result = await _controller.Login(userLoginDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(token);
    }

    [Fact]
    public async Task RefreshToken_ReturnsOkObjectResultWithToken()
    {
        // Arrange
        var token = _fixture.Create<string>();
        var task = Task.FromResult(token);

        _identityServiceMock.Setup(x => x.RefreshToken(It.IsAny<string>())).Returns(task);

        var mockContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, _fixture.Create<string>())
                }))
            }
        };
        var _controller = new IdentityController(_identityServiceMock.Object)
        {
            ControllerContext = mockContext
        };

        // Act
        var result = await _controller.RefreshToken();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(token);
    }
}