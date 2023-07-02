using FluentValidation;
using MusicApp.Identity.Application.DTOs;

namespace MusicApp.Identity.Application.Validators;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        var emptyUsernameError = "The field 'Username' is required.";
        var usernameLengthError = "The field 'Username' must be [5, 32] characters long.";
        var emptyPasswordError = "The field 'Password' is required.";
        var passwordLengthError = "The field 'Password' must be [8, 32] characters long.";

        RuleFor(c => c.Username)
            .NotEmpty().WithMessage(emptyUsernameError)
            .Length(5, 32).WithMessage(usernameLengthError);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(emptyPasswordError)
            .Length(8, 32).WithMessage(passwordLengthError);
    }
}
