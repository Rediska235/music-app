using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Application.Grpc;
using MusicApp.SongService.Application.Grpc.Protos;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.Tests.SongService.UnitTests.Commands;

public class CreateSongCommandHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<ISongRepository> _repositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<GrpcSong.GrpcSongClient> _clientMock = new();
    private readonly Mock<GrpcSongClient> _grpcClientMock;
    private readonly CreateSongCommandHandler _handler;
    public CreateSongCommandHandlerTests()
    {
        _grpcClientMock = new(_mapperMock.Object, _clientMock.Object);

        _handler = new(
            _repositoryMock.Object, 
            _mapperMock.Object, 
            _grpcClientMock.Object);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Handle_ShouldReturnCreateSong()
    {
        // Arrange
        var songInputDto = _fixture.Create<SongInputDto>();
        var artist = _fixture.Create<Artist>();
        var command = new CreateSongCommand(songInputDto, artist);
        var song = _fixture.Create<Song>();
        var songOutputDto = _fixture.Create<SongOutputDto>();

        _mapperMock.Setup(mapperMock => mapperMock.Map<Song>(command)).Returns(song);
        _mapperMock.Setup(mapperMock => mapperMock.Map<SongOutputDto>(song)).Returns(songOutputDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.Should().Be(songOutputDto);
    }
}