using FluentValidation;
using MusicApp.PlaylistService.Application.DTOs;

namespace MusicApp.PlaylistService.Application.Validators;

public class PlaylistInputDtoValidator : AbstractValidator<PlaylistInputDto>
{
    public PlaylistInputDtoValidator()
    {
        RuleFor(playlist => playlist.Name)
            .NotEmpty().WithMessage("The field 'Name' is required.")
            .Length(2, 32).WithMessage("The field 'Name' must be [2, 32] characters long.");
    }
}
