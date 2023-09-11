using AutoFixture;
using FluentAssertions;
using MusicApp.SongService.Application.CQRS.Commands.UpdateSong;
using MusicApp.SongService.Application.DTOs;

namespace MusicApp.Tests.SongService.UnitTests.Validators;

public class UpdateSongCommandValidatorTests
{
    private readonly UpdateSongCommandValidator _validator = new();
    private readonly Fixture _fixture = new();

    private const string emptyTitleErrorMessage = "The field 'Title' is required.";
    private const string wrongLengthTitleErrorMessage = "The field 'Title' must be [2, 32] characters long.";
    private const string validField = "12345678";

    public UpdateSongCommandValidatorTests()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void Validate_WhenTitleIsEmpty_ShouldHaveError()
    {
        // Arrange
        var song = new SongInputDto
        {
            Title = string.Empty
        };
        var id = _fixture.Create<Guid>();
        var command = new UpdateSongCommand(id, song);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Contain(emptyTitleErrorMessage)
            .And.Contain(wrongLengthTitleErrorMessage);
    }

    [Fact]
    public void Validate_WhenTitleIsTooShort_ShouldHaveError()
    {
        // Arrange
        var song = new SongInputDto
        {
            Title = "a"
        };
        var id = _fixture.Create<Guid>();
        var command = new UpdateSongCommand(id, song);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Be(wrongLengthTitleErrorMessage);
    }

    [Fact]
    public void Validate_WhenTitleIsTooLong_ShouldHaveError()
    {
        // Arrange
        var song = new SongInputDto
        {
            Title = "a".PadLeft(33, 'a')
        };
        var id = _fixture.Create<Guid>();
        var command = new UpdateSongCommand(id, song);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Be(wrongLengthTitleErrorMessage);
    }

    [Fact]
    public void Validate_WhenTitleIsValid_ShouldNotHaveError()
    {
        // Arrange
        var song = new SongInputDto
        {
            Title = validField
        };
        var id = _fixture.Create<Guid>();
        var command = new UpdateSongCommand(id, song);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
