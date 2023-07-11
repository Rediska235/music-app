using FluentValidation;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSong;

public class CreateSongCommandValidator : AbstractValidator<CreateSongCommand>
{
    public CreateSongCommandValidator()
    {
        RuleFor(c => c.Song.Title)
            .NotEmpty().WithMessage("The field 'Title' is required.")
            .Length(2, 32).WithMessage("The field 'Title' must be [2, 32] characters long.");
    }
}
