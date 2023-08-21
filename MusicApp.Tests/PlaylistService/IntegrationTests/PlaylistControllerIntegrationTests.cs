using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Infrastructure.Data;
using MusicApp.Tests.PlaylistService.IntegrationTests.Extensions;
using MusicApp.Tests.PlaylistService.IntegrationTests.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MusicApp.Tests.PlaylistService.IntegrationTests;

public class PlaylistControllerIntegrationTests : IDisposable
{

    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly AppDbContext _context;

    public PlaylistControllerIntegrationTests()
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
    public async Task GetPlaylistsAsync_ShouldReturnOkObjectResult()
    {
        // Arrange
        AddPlaylistToDatabase();

        // Act
        var response = await _client.GetAsync("/api/playlists");

        var resultPlaylist = JsonConvert.DeserializeObject<IEnumerable<PlaylistOutputDto>>(
            response.Content.ReadAsStringAsync().Result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        resultPlaylist.First().Should().BeEquivalentTo(TestData.PlaylistOutputDto);
    }

    [Fact]
    public async Task GetPlaylistByIdAsync_WhenPlaylistNotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        AddPlaylistToDatabase();

        var id = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/playlists/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPlaylistByIdAsync_ShouldReturnOkObjectResult()
    {
        // Arrange
        AddPlaylistToDatabase();

        var id = TestData.Playlist.Id;

        // Act
        var response = await _client.GetAsync($"/api/playlists/{id}");
        var resultPlaylist = JsonConvert.DeserializeObject<PlaylistOutputDto>(
            response.Content.ReadAsStringAsync().Result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        resultPlaylist.Should().BeEquivalentTo(TestData.PlaylistOutputDto);
    }

    [Fact]
    public async Task CreatePlaylistAsync_WhenIncorrectName_ShouldReturnInternalServerErrorResult()
    {
        // Arrange
        var playlistInputDto = new PlaylistInputDto
        {
            Name = "a",
            IsPrivate = false
        };
        var content = new StringContent(JsonConvert.SerializeObject(playlistInputDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/playlists", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task CreatePlaylistAsync_ShouldReturnCreatedAtActionResult()
    {
        // Arrange
        var content = new StringContent(JsonConvert.SerializeObject(TestData.PlaylistInputDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/playlists", content);
        var resultPlaylist = JsonConvert.DeserializeObject<PlaylistOutputDto>(
            response.Content.ReadAsStringAsync().Result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        resultPlaylist.Should().BeEquivalentTo(TestData.PlaylistInputDto);
    }

    [Fact]
    public async Task UpdatePlaylistAsync_WhenPlaylistNotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        AddPlaylistToDatabase();

        var id = Guid.NewGuid();
        var content = new StringContent(JsonConvert.SerializeObject(TestData.UpdatedPlaylist), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/playlists/{id}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdatePlaylistAsync_WhenIncorrectName_ShouldReturnInternalServerErrorResult()
    {
        // Arrange
        ClearDb();
        AddPlaylistToDatabase();

        var id = TestData.Playlist.Id;
        var playlistInputDto = new PlaylistInputDto
        {
            Name = "a",
            IsPrivate = false
        };
        var content = new StringContent(JsonConvert.SerializeObject(playlistInputDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/playlists/{id}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task UpdatePlaylistAsync_ShouldReturnOkObjectResult()
    {
        // Arrange
        ClearDb();
        AddPlaylistToDatabase();

        var id = TestData.Playlist.Id;
        var content = new StringContent(JsonConvert.SerializeObject(TestData.UpdatedPlaylist), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/playlists/{id}", content);
        var resultPlaylist = JsonConvert.DeserializeObject<PlaylistOutputDto>(
            response.Content.ReadAsStringAsync().Result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        resultPlaylist.Should().BeEquivalentTo(TestData.UpdatedPlaylist);
    }

    [Fact]
    public async Task DeletePlaylistAsync_WhenPlaylistNotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        AddPlaylistToDatabase();

        var id = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/playlists/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeletePlaylistAsync_ShouldReturnNoContentResult()
    {
        // Arrange
        AddPlaylistToDatabase();

        var id = TestData.Playlist.Id;

        // Act
        var response = await _client.DeleteAsync($"/api/playlists/{id}");
        var getResponse = await _client.GetAsync($"/api/playlists/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddSong_WhenPlaylistNotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        AddPlaylistToDatabase();
        AddSongToDatabase();

        var playlistId = Guid.NewGuid();
        var songId = TestData.Song.Id;

        // Act
        var response = await _client.PatchAsync($"/api/playlists/{playlistId}/add/{songId}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddSong_WhenSongNotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        ClearDb();
        AddPlaylistToDatabase();
        AddSongToDatabase();

        var playlistId = TestData.Playlist.Id;
        var songId = Guid.NewGuid();

        // Act
        var response = await _client.PatchAsync($"/api/playlists/{playlistId}/add/{songId}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddSong_ShouldReturnOkResult()
    {
        // Arrange
        AddPlaylistToDatabase();
        AddSongToDatabase();

        var playlistId = TestData.Playlist.Id;
        var songId = TestData.Song.Id;

        // Act
        var response = await _client.PatchAsync($"/api/playlists/{playlistId}/add/{songId}", null);
        var getResponse = await _client.GetAsync($"/api/playlists/{playlistId}");
        var resultPlaylist = JsonConvert.DeserializeObject<PlaylistOutputDto>(
            getResponse.Content.ReadAsStringAsync().Result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        resultPlaylist.Songs.First().Should().BeEquivalentTo(TestData.SongOutputDto);
    }

    [Fact]
    public async Task RemoveSong_WhenPlaylistNotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        AddPlaylistToDatabase();
        AddSongToDatabase();

        var playlistId = Guid.NewGuid();
        var songId = TestData.Song.Id;

        // Act
        var response = await _client.PatchAsync($"/api/playlists/{playlistId}/remove/{songId}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemoveSong_WhenSongNotFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        AddPlaylistToDatabase();
        AddSongToDatabase();

        var playlistId = TestData.Playlist.Id;
        var songId = Guid.NewGuid();

        // Act
        var response = await _client.PatchAsync($"/api/playlists/{playlistId}/remove/{songId}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemoveSong_ShouldReturnOkResult()
    {
        // Arrange
        AddPlaylistToDatabase();
        AddSongToDatabase();

        var playlistId = TestData.Playlist.Id;
        var songId = TestData.Song.Id;

        // Act
        await _client.PatchAsync($"/api/playlists/{playlistId}/add/{songId}", null);

        var response = await _client.PatchAsync($"/api/playlists/{playlistId}/remove/{songId}", null);
        var getResponse = await _client.GetAsync($"/api/playlists/{playlistId}");
        var resultPlaylist = JsonConvert.DeserializeObject<PlaylistOutputDto>(
            getResponse.Content.ReadAsStringAsync().Result);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        resultPlaylist.Songs.Should().BeEmpty();
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

    private void AddPlaylistToDatabase()
    {
        var playlist = _context.Playlists.FirstOrDefault(playlist => playlist.Id == TestData.Playlist.Id);
        if (playlist == null)
        {
            playlist = TestData.Playlist;

            var user = _context.Users.FirstOrDefault(user => user.Id == TestData.User.Id);
            if (user != null)
            {
                playlist.Creator = user;
            }

            _context.Playlists.Add(playlist);
            _context.SaveChanges();
        }
    }

    private void AddSongToDatabase()
    {
        var song = _context.Songs.FirstOrDefault(song => song.Id == TestData.Song.Id);
        if (song == null)
        {
            song = TestData.Song;

            var user = _context.Users.FirstOrDefault(user => user.Id == TestData.User.Id);
            if (user != null)
            {
                song.Artist = user;
            }

            _context.Songs.Add(song);
            _context.SaveChanges();
        }
    }

    private void ClearDb()
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == TestData.User.Id);
        if (user != null)
        {
            _context.Users.Remove(user);
        }

        var playlist = _context.Playlists.FirstOrDefault(playlist => playlist.Id == TestData.Playlist.Id);
        if (playlist != null)
        {
            _context.Playlists.Remove(playlist);
        }

        var song = _context.Songs.FirstOrDefault(song => song.Id == TestData.Song.Id);
        if (song != null)
        {
            _context.Songs.Remove(song);
        }

        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
        _client.Dispose();
        _factory.Dispose();
    }
}