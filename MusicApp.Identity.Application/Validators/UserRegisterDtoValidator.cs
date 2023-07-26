using FluentValidation;
using MusicApp.Identity.Application.DTOs;

namespace MusicApp.Identity.Application.Validators;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("The field 'Username' is required.")
            .Length(5, 32).WithMessage("The field 'Username' must be [5, 32] characters long.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("The field 'Password' is required.")
            .Length(8, 32).WithMessage("The field 'Password' must be [8, 32] characters long.");
    }
}
