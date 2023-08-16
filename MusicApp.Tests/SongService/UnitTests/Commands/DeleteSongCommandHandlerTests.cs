using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using MusicApp.SongService.Application.CQRS.Commands.DeleteSong;
using MusicApp.SongService.Application.Grpc;
using MusicApp.SongService.Application.Grpc.Protos;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services.Interfaces;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.Tests.SongService.UnitTests.Commands;

public class DeleteSongCommandHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<ISongRepository> _songRepositoryMock = new();
    private readonly Mock<IArtistService> _artistServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<GrpcSong.GrpcSongClient> _clientMock = new();
    private readonly Mock<GrpcSongClient> _grpcClientMock;
    private readonly DeleteSongCommandHandler _handler;

    public DeleteSongCommandHandlerTests()
    {
        _grpcClientMock = new(_mapperMock.Object, _clientMock.Object);

        _handler = new(
            _songRepositoryMock.Object,
            _artistServiceMock.Object,
            _grpcClientMock.Object);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Handle_WhenSongNotFound_ShouldThrowException()
    {
        // Arrange
        var command = _fixture.Create<DeleteSongCommand>();

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await act.Should().ThrowAsync<SongNotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenNotYourSong_ShouldThrowException()
    {
        // Arrange
        var command = _fixture.Create<DeleteSongCommand>();
        var song = _fixture.Create<Song>();

        _songRepositoryMock.Setup(songRepositoryMock => 
            songRepositoryMock.GetByIdAsync(command.Id, _cancellationToken))
                .ReturnsAsync(song);

        _artistServiceMock.Setup(artistService => artistService.ValidateArtistAndThrow(song)).Throws<NotYourSongException>();

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await act.Should().ThrowAsync<NotYourSongException>();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessfulTask()
    {
        // Arrange
        var command = _fixture.Create<DeleteSongCommand>();
        var song = _fixture.Create<Song>();

        _songRepositoryMock.Setup(songRepositoryMock =>
            songRepositoryMock.GetByIdAsync(command.Id, _cancellationToken))
                .ReturnsAsync(song);

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

}
