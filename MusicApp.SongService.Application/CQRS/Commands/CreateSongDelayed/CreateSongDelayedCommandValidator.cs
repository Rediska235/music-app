using FluentValidation;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSongDelayed;

public class CreateSongDelayedCommandValidator : AbstractValidator<CreateSongDelayedCommand>
{
    public CreateSongDelayedCommandValidator()
    {
        RuleFor(command => command.delayedSongInputDto.Title)
            .NotEmpty().WithMessage("The field 'Title' is required.")
            .Length(2, 32).WithMessage("The field 'Title' must be [2, 32] characters long.");
    }
}
