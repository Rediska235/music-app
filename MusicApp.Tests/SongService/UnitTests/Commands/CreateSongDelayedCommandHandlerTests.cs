using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using MusicApp.SongService.Application.CQRS.Commands.CreateSongDelayed;
using MusicApp.SongService.Application.Repositories;

namespace MusicApp.Tests.SongService.UnitTests.Commands;

public class CreateSongDelayedCommandHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<ISongRepository> _songRepositoryMock = new();
    private readonly Mock<IArtistRepository> _artistRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly CreateSongDelayedCommandHandler _handler;
    public CreateSongDelayedCommandHandlerTests()
    {
        _handler = new(
            _songRepositoryMock.Object,
            _artistRepositoryMock.Object,
            _mapperMock.Object);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    /*[Fact]
    public async Task Handle_ShouldReturnSuccessfulTask()
    {
        // Arrange
        var command = _fixture.Create<CreateSongDelayedCommand>();

        //Act
        //System.InvalidOperationException: Current JobStorage instance has not been initialized yet. [BackgroundJob.Schedule(job, delay)]
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }*/
}