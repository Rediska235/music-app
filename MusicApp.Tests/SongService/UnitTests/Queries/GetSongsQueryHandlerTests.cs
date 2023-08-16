using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using MusicApp.SongService.Application.CQRS.Queries.GetSongs;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.Tests.SongService.UnitTests.Queries;

public class GetSongsQueryHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<ISongRepository> _songRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly GetSongsQueryHandler _handler;

    public GetSongsQueryHandlerTests()
    {
        _handler = new(_songRepositoryMock.Object, _mapperMock.Object);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Handle_ShouldReturnSongs()
    {
        // Arrange
        var query = _fixture.Create<GetSongsQuery>();
        var songs = _fixture.CreateMany<Song>();
        var outputSongs = _fixture.CreateMany<SongOutputDto>();

        _songRepositoryMock.Setup(songRepositoryMock =>
            songRepositoryMock.GetAsync(_cancellationToken)).ReturnsAsync(songs);

        _mapperMock.Setup(mapperMock => mapperMock.Map<IEnumerable<SongOutputDto>>(songs)).Returns(outputSongs);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(outputSongs);
    }
}