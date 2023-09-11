using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MusicApp.Identity.Application.Services.Implementations;
using MusicApp.Identity.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace MusicApp.Tests.Identity.UnitTests.Services;

public class JwtServiceTests
{
    private readonly Mock<HttpContext> _httpContextMock = new();
    private readonly Mock<HttpResponse> _httpResponseMock = new();
    private readonly Mock<IResponseCookies> _cookiesMock = new();
    private readonly Fixture _fixture = new();
    private readonly JwtService _jwtService = new();

    public JwtServiceTests()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnRefreshToken()
    {
        //Arrange

        //Act
        var refreshToken = _jwtService.GenerateRefreshToken();

        //Assert
        refreshToken.Should().NotBeNull();
        refreshToken.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void SetRefreshToken_ShouldSetRefreshTokenToUser()
    {
        //Arrange
        var refreshToken = _fixture.Create<RefreshToken>();
        var user = _fixture.Create<User>();

        _httpContextMock.SetupGet(context => context.Response).Returns(_httpResponseMock.Object);
        _httpResponseMock.SetupGet(response => response.Cookies).Returns(_cookiesMock.Object);

        //Act
        _jwtService.SetRefreshToken(refreshToken, _httpContextMock.Object, user);

        //Assert
        user.RefreshToken.Should().NotBeNullOrWhiteSpace();
        user.RefreshToken.Should().Be(refreshToken.Token);
    }

    [Fact]
    public void CreateToken_WhenUserIsNotArtist_ShouldReturnTokenWithEmptyRoleClaims()
    {
        //Arrange
        var user = new User();
        var secretKey = "j80sNF5L0O3W09CVlKVlO0KbxonrdPGOGDgs33ZIRTchFiwbooLpUIYkl3gM45HI";

        //Act
        var result = _jwtService.CreateToken(user, secretKey);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result);
        var roleClaims = jwt.Claims.Where(claim => claim.Type == "Role");

        //Assert
        result.Should().BeOfType<string>()
            .And.NotBeNullOrWhiteSpace();
        roleClaims.Should().BeEmpty();
    }

    [Fact]
    public void CreateToken_WhenUserIsNotArtist_ShouldReturnTokenWithArtistRoleClaim()
    {
        //Arrange
        var user = new User
        {
            Roles = new List<Role>
            {
                new Role
                {
                    Title = "artist"
                }
            }
        };
        var secretKey = "j80sNF5L0O3W09CVlKVlO0KbxonrdPGOGDgs33ZIRTchFiwbooLpUIYkl3gM45HI";

        //Act
        var result = _jwtService.CreateToken(user, secretKey);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result);
        var roleClaims = jwt.Claims.Where(claim => claim.Type == "Role");

        //Assert
        result.Should().BeOfType<string>()
            .And.NotBeNullOrWhiteSpace();
        roleClaims.Should().HaveCount(1)
            .And.Contain(claim => claim.Value == "artist");
    }
}