namespace MusicApp.Identity.Domain.Exceptions;

[Serializable]
public class InvalidUsernameOrPasswordException : Exception
{
    private static string? DefaultMessage = "Invalid username or password.";

    public InvalidUsernameOrPasswordException() : base(DefaultMessage) { }
    public InvalidUsernameOrPasswordException(string message)
        : base(message) { }
    public InvalidUsernameOrPasswordException(string message, Exception inner)
        : base(message, inner) { }
}
