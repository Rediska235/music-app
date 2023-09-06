using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Application.Services.Interfaces;
using MusicApp.PlaylistService.Web.Controllers;

namespace MusicApp.Tests.PlaylistService.UnitTests.Controllers;

public class PlaylistControllerTests
{
    private readonly Mock<IPlaylistsService> _playlistServiceMock = new();
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly PlaylistsController _controller;

    public PlaylistControllerTests()
    {
        _controller = new(_playlistServiceMock.Object);
    }

    [Fact]
    public async Task GetPlaylistsAsync_ShouldReturnOkObjectResultWithPlaylists()
    {
        // Arrange
        var playlists = _fixture.CreateMany<PlaylistOutputDto>();
        _playlistServiceMock.Setup(x => x.GetPlaylistsAsync(_cancellationToken)).ReturnsAsync(playlists);

        // Act
        var result = await _controller.GetPlaylists(_cancellationToken);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(playlists);
    }

    [Fact]
    public async Task GetPlaylistByIdAsync_ShouldReturnOkObjectResultWithPlaylist()
    {
        // Arrange
        var playlist = _fixture.Create<PlaylistOutputDto>();
        var id = _fixture.Create<Guid>();
        _playlistServiceMock.Setup(x => x.GetPlaylistByIdAsync(id, _cancellationToken)).ReturnsAsync(playlist);

        // Act
        var result = await _controller.GetPlaylistById(id, _cancellationToken);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(playlist);
    }

    [Fact]
    public async Task CreatePlaylistAsync_ShouldReturnCreatedAtActionResultWithCreatedPlaylist()
    {
        // Arrange
        var playlist = _fixture.Create<PlaylistOutputDto>();
        var playlistInputDto = _fixture.Create<PlaylistInputDto>();
        _playlistServiceMock.Setup(x => x.CreatePlaylistAsync(playlistInputDto, _cancellationToken)).ReturnsAsync(playlist);

        // Act
        var result = await _controller.CreatePlaylist(playlistInputDto, _cancellationToken);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        result.As<CreatedAtActionResult>().Value.Should().Be(playlist);
    }

    [Fact]
    public async Task UpdatePlaylistAsync_ShouldReturnOkObjectResultWithUpdatedPlaylist()
    {
        // Arrange
        var playlist = _fixture.Create<PlaylistOutputDto>();
        var playlistInputDto = _fixture.Create<PlaylistInputDto>();
        var id = _fixture.Create<Guid>();
        _playlistServiceMock.Setup(x => x.UpdatePlaylistAsync(id, playlistInputDto, _cancellationToken)).ReturnsAsync(playlist);

        // Act
        var result = await _controller.UpdatePlaylist(id, playlistInputDto, _cancellationToken);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(playlist);
    }

    [Fact]
    public async Task DeletePlaylistAsync_ShouldReturnNoContentResult()
    {
        // Arrange
        var id = _fixture.Create<Guid>();

        // Act
        var result = await _controller.DeletePlaylist(id, _cancellationToken);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task AddSong_ShouldReturnOkResult()
    {
        // Arrange
        var playlistId = _fixture.Create<Guid>();
        var songId = _fixture.Create<Guid>();

        // Act
        var result = await _controller.AddSong(playlistId, songId, _cancellationToken);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task RemoveSong_ShouldReturnOkResult()
    {
        // Arrange
        var playlistId = _fixture.Create<Guid>();
        var songId = _fixture.Create<Guid>();

        // Act
        var result = await _controller.RemoveSong(playlistId, songId, _cancellationToken);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}