using AutoFixture;
using FluentAssertions;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Application.Validators;

namespace MusicApp.Tests.Identity.UnitTests.Validators;

public class PlaylistInputDtoValidatorTests
{
    private readonly PlaylistInputDtoValidator _validator = new();
    private readonly Fixture _fixture = new();

    private const string emptyNameErrorMessage = "The field 'Name' is required.";
    private const string wrongLengthNameErrorMessage = "The field 'Name' must be [2, 32] characters long.";
    private const string validField = "12345678";

    public PlaylistInputDtoValidatorTests()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void Validate_WhenNameIsEmpty_ShouldHaveError()
    {
        // Arrange
        var playlistInputDto = new PlaylistInputDto
        {
            Name = string.Empty
        };

        // Act
        var result = _validator.Validate(playlistInputDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Contain(emptyNameErrorMessage)
            .And.Contain(wrongLengthNameErrorMessage);
    }

    [Fact]
    public void Validate_WhenNameIsTooShort_ShouldHaveError()
    {
        // Arrange
        var playlistInputDto = new PlaylistInputDto
        {
            Name = "a"
        };

        // Act
        var result = _validator.Validate(playlistInputDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Be(wrongLengthNameErrorMessage);
    }

    [Fact]
    public void Validate_WhenNameIsTooLong_ShouldHaveError()
    {
        // Arrange
        var playlistInputDto = new PlaylistInputDto
        {
            Name = "a".PadLeft(33, 'a')
        };

        // Act
        var result = _validator.Validate(playlistInputDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Be(wrongLengthNameErrorMessage);
    }

    [Fact]
    public void Validate_WhenNameIsValid_ShouldNotHaveError()
    {
        // Arrange
        var playlistInputDto = new PlaylistInputDto
        {
            Name = validField
        };

        // Act
        var result = _validator.Validate(playlistInputDto);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
