using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Identity.Infrastructure.Data;
using MusicApp.Tests.Identity.IntegrationTests.Extensions;
using MusicApp.Tests.Identity.IntegrationTests.Helpers;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MusicApp.Tests.Identity.IntegrationTests.Controllers;

public class IdentityControllerIntegrationTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly AppDbContext _context;

    private const string wrongLengthUsernameErrorMessage = "The field 'Username' must be [5, 32] characters long.";
    private const string wrongLengthPasswordErrorMessage = "The field 'Password' must be [8, 32] characters long.";

    public IdentityControllerIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("InMemoryDbForTesting")
            .Options;
        _context = new AppDbContext(options);

        _factory = new CustomWebApplicationFactory();

        var claimsProvider = TestClaimsProvider.WithUserClaims();
        _client = _factory.CreateClientWithTestAuth(claimsProvider);
    }

    [Fact]
    public async Task Register_WhenIncorrectUsernameOrPassword_ShouldReturnInternalServerError()
    {
        // Arrange
        var content = new StringContent(JsonConvert.SerializeObject(TestData.UserWithIncorrectCreds), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/identity/register", content);
        var result = await response.Content.ReadAsStringAsync();
        result = result.Replace(@"\u0027", "'");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.Should().Contain(wrongLengthUsernameErrorMessage)
            .And.Contain(wrongLengthPasswordErrorMessage);
    }

    [Fact]
    public async Task Register_ReturnsOkObjectResultWithUser()
    {
        // Arrange
        var content = new StringContent(JsonConvert.SerializeObject(TestData.UserForRegister), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/identity/register", content);
        var result = JsonConvert.DeserializeObject<User>(
            response.Content.ReadAsStringAsync().Result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Username.Should().Be(TestData.UserForRegister.Username);
    }

    [Fact]
    public async Task Login_WhenIncorrectUsernameOrPassword_ShouldReturnInternalServerError()
    {
        // Arrange
        var content = new StringContent(JsonConvert.SerializeObject(TestData.UserWithIncorrectCreds), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/identity/login", content);
        var result = await response.Content.ReadAsStringAsync();
        result = result.Replace(@"\u0027", "'");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.Should().Contain(wrongLengthUsernameErrorMessage)
            .And.Contain(wrongLengthPasswordErrorMessage);
    }

    [Fact]
    public async Task Login_WhenWrongPassword_ShouldReturnBadRequest()
    {
        // Arrange
        var registerContent = new StringContent(JsonConvert.SerializeObject(TestData.UserForLogin2), Encoding.UTF8, "application/json");
        var loginContent = new StringContent(JsonConvert.SerializeObject(TestData.UserForLogin3), Encoding.UTF8, "application/json");

        // Act
        await _client.PostAsync("/api/identity/register", registerContent);
        var response = await _client.PostAsync("/api/identity/login", loginContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ReturnsOkObjectResultWithToken()
    {
        // Arrange
        var content = new StringContent(JsonConvert.SerializeObject(TestData.UserForLogin), Encoding.UTF8, "application/json");
        var handler = new JwtSecurityTokenHandler();

        // Act
        await _client.PostAsync("/api/identity/register", content);
        var response = await _client.PostAsync("/api/identity/login", content);

        var result = response.Content.ReadAsStringAsync().Result;
        var jwtSecurityToken = handler.ReadJwtToken(result);
        var username = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        username.Should().Be(TestData.UserForLogin.Username);
    }

    [Fact]
    public async Task RefreshToken_WhenInvalidRefreshToken_ShouldReturnBadRequestResult()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/api/identity/refresh-token");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RefreshToken_ReturnsOkObjectResultWithToken()
    {
        // Arrange
        AddUserToDatabase();
        
        var message = new HttpRequestMessage(HttpMethod.Get, "/api/identity/refresh-token");
        message.Headers.Add("Cookie", "refreshToken=refreshToken");

        var jwtHandler = new JwtSecurityTokenHandler();

        // Act
        var response = await _client.SendAsync(message);

        var result = response.Content.ReadAsStringAsync().Result;
        var jwtSecurityToken = jwtHandler.ReadJwtToken(result);
        var username = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        username.Should().Be(TestData.User.Username);
    }

    private void AddUserToDatabase()
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == TestData.User.Id);
        if (user == null)
        {
            _context.Users.Add(TestData.User);
            _context.SaveChanges();
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        _client.Dispose();
        _factory.Dispose();
    }
}