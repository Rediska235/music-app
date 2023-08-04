namespace MusicApp.PlaylistService.Domain.Exceptions;

[Serializable]
public class UserNotFoundException : Exception
{
    private static string? DefaultMessage = "User not found.";

    public UserNotFoundException() : base(DefaultMessage) { }

    public UserNotFoundException(string message)
        : base(message) { }

    public UserNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}
