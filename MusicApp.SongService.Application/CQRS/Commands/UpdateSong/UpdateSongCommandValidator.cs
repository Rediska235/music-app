using FluentValidation;

namespace MusicApp.SongService.Application.CQRS.Commands.UpdateSong;

public class UpdateSongCommandValidator : AbstractValidator<UpdateSongCommand>
{
    public UpdateSongCommandValidator()
    {
        RuleFor(command => command.Song.Title)
            .NotEmpty().WithMessage("The field 'Title' is required.")
            .Length(2, 32).WithMessage("The field 'Title' must be [2, 32] characters long.");
    }
}
