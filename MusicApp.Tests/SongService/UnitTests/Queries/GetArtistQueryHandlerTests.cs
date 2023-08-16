using AutoFixture;
using FluentAssertions;
using Moq;
using MusicApp.SongService.Application.CQRS.Queries.GetArtist;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Application.Services.Interfaces;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.Tests.SongService.UnitTests.Queries;

public class GetArtistQueryHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<IArtistRepository> _artistRepositoryMock = new();
    private readonly Mock<IArtistService> _artistServiceMock = new();
    private readonly GetArtistQueryHandler _handler;

    public GetArtistQueryHandlerTests()
    {
        _handler = new(_artistRepositoryMock.Object, _artistServiceMock.Object);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Handle_WhenArtistNotFound_ShouldThrowException()
    {
        // Arrange
        var query = _fixture.Create<GetArtistQuery>();

        // Act
        var act = async () => await _handler.Handle(query, _cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ArtistNotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldReturnArtist()
    {
        // Arrange
        var query = _fixture.Create<GetArtistQuery>();
        var artist = _fixture.Create<Artist>();

        _artistRepositoryMock.Setup(artistRepositoryMock =>
            artistRepositoryMock.GetArtistByUsernameAsync(It.IsAny<string>(), _cancellationToken))
                .ReturnsAsync(artist);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        // Assert
        result.Should().Be(artist);
    }
}