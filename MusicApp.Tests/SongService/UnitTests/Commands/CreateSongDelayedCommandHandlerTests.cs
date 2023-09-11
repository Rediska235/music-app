using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Hangfire;
using Hangfire.Common;
using Moq;
using MusicApp.SongService.Application.CQRS.Commands.CreateSongDelayed;
using MusicApp.SongService.Application.Repositories;
using System.Linq.Expressions;
using System.Threading;

namespace MusicApp.Tests.SongService.UnitTests.Commands;

public class CreateSongDelayedCommandHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<ISongRepository> _songRepositoryMock = new();
    private readonly Mock<IArtistRepository> _artistRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IBackgroundJobClient> _backgroundJobClientMock = new();
    private readonly CreateSongDelayedCommandHandler _handler;

    public CreateSongDelayedCommandHandlerTests()
    {
        _handler = new(
            _songRepositoryMock.Object,
            _artistRepositoryMock.Object,
            _mapperMock.Object,
            _backgroundJobClientMock.Object);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Handle_ShouldCreateSongWithDelay()
    {
        // Arrange
        var command = _fixture.Create<CreateSongDelayedCommand>();

        _backgroundJobClientMock.Setup(client => client.Create(It.IsAny<Job>(), It.IsAny<Hangfire.States.IState>()));

        //Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }
}