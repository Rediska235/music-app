using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Infrastructure.Data;
using MusicApp.Tests.SongService.IntegrationTests.Extensions;
using MusicApp.Tests.SongService.IntegrationTests.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MusicApp.Tests.SongService.IntegrationTests;

public class SongControllerIntegrationTests : IDisposable
{

    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly AppDbContext _context;

    public SongControllerIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("InMemoryDbForTesting")
            .Options;
        _context = new AppDbContext(options);

        AddUserToDatabase();

        _factory = new CustomWebApplicationFactory();

        var claimsProvider = TestClaimsProvider.WithUserClaims();
        _client = _factory.CreateClientWithTestAuth(claimsProvider);
    }

    [Fact]
    public async Task GetSongsAsync_ShouldReturnOkObjectResultWithSongs()
    {
        // Arrange
        AddSongToDatabase();

        // Act
        var response = await _client.GetAsync("/api/songs");

        var resultPlaylist = JsonConvert.DeserializeObject<IEnumerable<SongOutputDto>>(
            response.Content.ReadAsStringAsync().Result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        resultPlaylist.First().Should().BeEquivalentTo(TestData.SongOutputDto);
    }

    [Fact]
    public async Task GetSongByIdAsync_WhenSongNotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        AddSongToDatabase();

        var id = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/songs/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSongByIdAsync_ShouldReturnOkObjectResultWithSong()
    {
        // Arrange
        AddSongToDatabase();

        var id = TestData.Song.Id;

        // Act
        var response = await _client.GetAsync($"/api/songs/{id}");
        var resultPlaylist = JsonConvert.DeserializeObject<SongOutputDto>(
            response.Content.ReadAsStringAsync().Result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        resultPlaylist.Should().BeEquivalentTo(TestData.SongOutputDto);
    }

    [Fact]
    public async Task CreateSongAsync_WhenIncorrectTitle_ShouldReturnInternalServerErrorResult()
    {
        // Arrange
        var content = new StringContent(JsonConvert.SerializeObject(TestData.SongWithIncorrectTitle), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/songs", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task CreateSongAsync_ShouldReturnCreatedAtActionResultWithCreatedSong()
    {
        // Arrange
        var content = new StringContent(JsonConvert.SerializeObject(TestData.SongInputDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/songs", content);
        var resultPlaylist = JsonConvert.DeserializeObject<SongOutputDto>(
            response.Content.ReadAsStringAsync().Result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        resultPlaylist.Should().BeEquivalentTo(TestData.SongInputDto);
    }

    [Fact]
    public async Task UpdateSongAsync_WhenSongNotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        AddSongToDatabase();

        var id = Guid.NewGuid();
        var content = new StringContent(JsonConvert.SerializeObject(TestData.SongInputDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/songs/{id}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateSongAsync_WhenIncorrectTitle_ShouldReturnInternalServerErrorResult()
    {
        // Arrange
        AddSongToDatabase();

        var id = TestData.Song.Id;
        var content = new StringContent(JsonConvert.SerializeObject(TestData.SongWithIncorrectTitle), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/songs/{id}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task UpdateSongAsync_ShouldReturnOkObjectResultWithUpdatedSong()
    {
        // Arrange
        AddSongToDatabase();

        var id = TestData.Song.Id;
        var content = new StringContent(JsonConvert.SerializeObject(TestData.SongInputDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/songs/{id}", content);
        var resultPlaylist = JsonConvert.DeserializeObject<SongOutputDto>(
            response.Content.ReadAsStringAsync().Result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        resultPlaylist.Should().BeEquivalentTo(TestData.SongInputDto);
    }

    [Fact]
    public async Task DeleteSongAsync_WhenSongNotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        AddSongToDatabase();

        var id = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/songs/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteSongAsync_ShouldReturnNoContentResult()
    {
        // Arrange
        AddSongToDatabase();

        var id = TestData.Song.Id;

        // Act
        var response = await _client.DeleteAsync($"/api/songs/{id}");
        var getResponse = await _client.GetAsync($"/api/songs/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private void AddUserToDatabase()
    {
        var artist = _context.Artists.FirstOrDefault(artist => artist.Id == TestData.Artist.Id);
        if (artist == null)
        {
            _context.Artists.Add(TestData.Artist);
            _context.SaveChanges();
        }
    }

    private void AddSongToDatabase()
    {
        var song = _context.Songs.FirstOrDefault(song => song.Id == TestData.Song.Id);
        if (song == null)
        {
            song = TestData.Song;

            var artist = _context.Artists.FirstOrDefault(artist => artist.Id == TestData.Artist.Id);
            if (artist != null)
            {
                song.Artist = artist;
            }

            _context.Songs.Add(song);
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