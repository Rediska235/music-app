using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.CQRS.Commands.CreateSongDelayed;
using MusicApp.SongService.Application.CQRS.Commands.DeleteSong;
using MusicApp.SongService.Application.CQRS.Commands.UpdateSong;
using MusicApp.SongService.Application.CQRS.Queries.GetSongById;
using MusicApp.SongService.Application.CQRS.Queries.GetSongs;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Web.Controllers;

namespace MusicApp.Tests.SongService.UnitTests.Controllers;

public class SongControllerTests
{
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly SongsController _controller;

    public SongControllerTests()
    {
        _controller = new(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetSongs_ShouldReturnOkObjectResultWithSongs()
    {
        // Arrange
        var songs = _fixture.CreateMany<SongOutputDto>();
        var task = Task.FromResult(songs);

        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetSongsQuery>(), _cancellationToken)).Returns(task);

        // Act
        var result = await _controller.GetSongs(_cancellationToken);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(songs);
    }

    [Fact]
    public async Task GetSongById_ShouldReturnOkObjectResultWithSong()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var song = _fixture.Create<SongOutputDto>();
        var task = Task.FromResult(song);

        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetSongByIdQuery>(), _cancellationToken)).Returns(task);

        // Act
        var result = await _controller.GetSongById(id, _cancellationToken);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(song);
    }

    [Fact]
    public async Task CreateSong_ShouldReturnCreatedAtActionResultWithCreatedSong()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var songInputDto = _fixture.Create<SongInputDto>();
        var songOutputDto = _fixture.Create<SongOutputDto>();
        var task = Task.FromResult(songOutputDto);

        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CreateSongCommand>(), _cancellationToken)).Returns(task);

        // Act
        var result = await _controller.CreateSong(songInputDto, _cancellationToken);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        result.As<CreatedAtActionResult>().Value.Should().Be(songOutputDto);
    }

    [Fact]
    public async Task CreateSongDelayed_ShouldReturnOkObjectResult()
    {
        // Arrange
        var delayedSongInputDto = _fixture.Create<DelayedSongInputDto>();

        // Act
        var result = await _controller.CreateSongDelayed(delayedSongInputDto, _cancellationToken);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeOfType<string>();
    }

    [Fact]
    public async Task UpdateSong_ShouldReturnOkObjectResultWithUpdatedSong()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var songInputDto = _fixture.Create<SongInputDto>();
        var songOutputDto = _fixture.Create<SongOutputDto>();
        var task = Task.FromResult(songOutputDto);

        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<UpdateSongCommand>(), _cancellationToken)).Returns(task);

        // Act
        var result = await _controller.UpdateSong(id, songInputDto, _cancellationToken);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(songOutputDto);
    }

    [Fact]
    public async Task DeleteSong_ShouldReturnNoContentResult()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var songOutputDto = _fixture.Create<SongOutputDto>();
        var task = Task.FromResult(songOutputDto);

        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<DeleteSongCommand>(), _cancellationToken)).Returns(task);

        // Act
        var result = await _controller.DeleteSong(id, _cancellationToken);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}