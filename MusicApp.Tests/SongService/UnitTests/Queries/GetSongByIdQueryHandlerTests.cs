using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using MusicApp.SongService.Application.CQRS.Queries.GetSongById;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.Tests.SongService.UnitTests.Queries;

public class GetSongByIdQueryHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<ISongRepository> _songRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly GetSongByIdQueryHandler _handler;

    public GetSongByIdQueryHandlerTests()
    {
        _handler = new(_songRepositoryMock.Object, _mapperMock.Object);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Handle_WhenSongNotFound_ShouldThrowException()
    {
        // Arrange
        var query = _fixture.Create<GetSongByIdQuery>();

        // Act
        var act = async () => await _handler.Handle(query, _cancellationToken);

        // Assert
        await act.Should().ThrowAsync<SongNotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldReturnSong()
    {
        // Arrange
        var query = _fixture.Create<GetSongByIdQuery>();
        var song = _fixture.Create<Song>();
        var songOutputDto = _fixture.Create<SongOutputDto>();

        _songRepositoryMock.Setup(songRepositoryMock =>
            songRepositoryMock.GetByIdAsync(It.IsAny<Guid>(), _cancellationToken))
                .ReturnsAsync(song);

        _mapperMock.Setup(mapperMock => mapperMock.Map<SongOutputDto>(song)).Returns(songOutputDto);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        // Assert
        result.Should().Be(songOutputDto);
    }
}