using AutoFixture;
using FluentAssertions;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Application.Validators;

namespace MusicApp.Tests.Identity.UnitTests.Validators;

public class UserRegisterDtoValidatorTests
{
    private readonly UserRegisterDtoValidator _validator = new();
    private readonly Fixture _fixture = new();

    private const string emptyUsernameErrorMessage = "The field 'Username' is required.";
    private const string wrongLengthUsernameErrorMessage = "The field 'Username' must be [5, 32] characters long.";
    private const string emptyPasswordErrorMessage = "The field 'Password' is required.";
    private const string wrongLengthPasswordErrorMessage = "The field 'Password' must be [8, 32] characters long.";
    private const string validField = "12345678";

    public UserRegisterDtoValidatorTests()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void Validate_WhenUsernameIsEmpty_ShouldHaveError()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = string.Empty,
            Password = validField
        };

        // Act
        var result = _validator.Validate(userRegisterDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Contain(emptyUsernameErrorMessage)
            .And.Contain(wrongLengthUsernameErrorMessage);
    }

    [Fact]
    public void Validate_WhenUsernameIsTooShort_ShouldHaveError()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = "abc",
            Password = validField
        };

        // Act
        var result = _validator.Validate(userRegisterDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Be(wrongLengthUsernameErrorMessage);
    }

    [Fact]
    public void Validate_WhenUsernameIsTooLong_ShouldHaveError()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = "a".PadLeft(33, 'a'),
            Password = validField
        };

        // Act
        var result = _validator.Validate(userRegisterDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Be(wrongLengthUsernameErrorMessage);
    }

    [Fact]
    public void Validate_WhenPasswordIsEmpty_ShouldHaveError()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = validField,
            Password = string.Empty
        };

        // Act
        var result = _validator.Validate(userRegisterDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Contain(emptyPasswordErrorMessage)
            .And.Contain(wrongLengthPasswordErrorMessage);
    }

    [Fact]
    public void Validate_WhenPasswordIsTooShort_ShouldHaveError()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = validField,
            Password = "abc"
        };

        // Act
        var result = _validator.Validate(userRegisterDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Be(wrongLengthPasswordErrorMessage);
    }

    [Fact]
    public void Validate_WhenPasswordIsTooLong_ShouldHaveError()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = validField,
            Password = "a".PadLeft(33, 'a')
        };

        // Act
        var result = _validator.Validate(userRegisterDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ToString().Should().Be(wrongLengthPasswordErrorMessage);
    }

    [Fact]
    public void Validate_WhenAllFieldsAreValid_ShouldNotHaveError()
    {
        // Arrange
        var userRegisterDto = new UserRegisterDto
        {
            Username = validField,
            Password = validField
        };

        // Act
        var result = _validator.Validate(userRegisterDto);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
