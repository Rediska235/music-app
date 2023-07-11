using FluentValidation;
using MusicApp.Identity.Application.DTOs;

namespace MusicApp.Identity.Application.Validators;

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty().WithMessage("The field 'Username' is required.")
            .Length(5, 32).WithMessage("The field 'Username' must be [5, 32] characters long.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("The field 'Password' is required.")
            .Length(8, 32).WithMessage("The field 'Password' must be [8, 32] characters long.");
    }
}
