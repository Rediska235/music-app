using FluentValidation;

namespace MusicApp.SongService.Application.CQRS.Commands.CreateSong;

public class CreateSongCommandValidator : AbstractValidator<CreateSongCommand>
{
    public CreateSongCommandValidator()
    {
        var emptyTitleError = "The field 'Title' is required.";
        var titleLengthError = "The field 'Title' must be [2, 32] characters long.";

        RuleFor(c => c.Song.Title)
            .NotEmpty().WithMessage(emptyTitleError)
            .Length(2, 32).WithMessage(titleLengthError);
    }

    public void ThrowsExceptionIfRequestIsNotValid(CreateSongCommand request)
    {
        var result = Validate(request);
        if (!result.IsValid)
        {
            var errorList = "";
            foreach (var failure in result.Errors)
            {
                errorList += failure.ErrorMessage + ';';
            }
            errorList = errorList.Remove(errorList.Length - 1);

            throw new ArgumentException(errorList);
        }
    }
}
