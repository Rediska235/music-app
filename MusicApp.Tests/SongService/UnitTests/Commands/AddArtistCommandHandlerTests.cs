using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using MusicApp.SongService.Application.CQRS.Commands.AddArtist;
using MusicApp.SongService.Application.Repositories;

namespace MusicApp.Tests.SongService.UnitTests.Commands;

public class AddArtistCommandHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<IArtistRepository> _repositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly AddArtistCommandHandler _handler;

    public AddArtistCommandHandlerTests()
    {
        _handler = new(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddArtist()
    {
        // Arrange
        var command = _fixture.Create<AddArtistCommand>();

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }
}