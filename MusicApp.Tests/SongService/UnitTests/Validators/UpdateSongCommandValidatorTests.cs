using AutoFixture;
using FluentAssertions;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.CQRS.Commands.UpdateSong;
using MusicApp.SongService.Application.DTOs;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.Tests.SongService.UnitTests.Validators;

public class UpdateSongCommandValidatorTests
{
    private readonly UpdateSongCommandValidator _validator = new();
    private readonly Fixture _fixture = new();

    private const string emptyTitleErrorMessage = "The field 'Title' is required.";
    private const string wrongLengthTitleErrorMessage = "The field 'Title' must be [2, 32] characters long.";

    public UpdateSongCommandValidatorTests()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void Validate_WhenTitleIsEmpty_ShouldHaveError()
    {
        var song = new SongInputDto
        {
            Title = string.Empty
        };
        var id = _fixture.Create<Guid>();

        var command = new UpdateSongCommand(id, song);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.ToString().Should().Contain(emptyTitleErrorMessage)
            .And.Contain(wrongLengthTitleErrorMessage);
    }

    [Fact]
    public void Validate_WhenTitleIsTooShort_ShouldHaveError()
    {
        var song = new SongInputDto
        {
            Title = "a"
        };
        var id = _fixture.Create<Guid>();

        var command = new UpdateSongCommand(id, song);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.ToString().Should().Be(wrongLengthTitleErrorMessage);
    }

    [Fact]
    public void Validate_WhenTitleIsTooLong_ShouldHaveError()
    {
        var song = new SongInputDto
        {
            Title = "a".PadLeft(33, 'a')
        };
        var id = _fixture.Create<Guid>();

        var command = new UpdateSongCommand(id, song);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.ToString().Should().Be(wrongLengthTitleErrorMessage);
    }

    [Fact]
    public void Validate_WhenTitleIsCorrect_ShouldBeValid()
    {
        var song = new SongInputDto
        {
            Title = "a".PadLeft(12, 'a')
        };
        var id = _fixture.Create<Guid>();

        var command = new UpdateSongCommand(id, song);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
