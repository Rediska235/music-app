namespace MusicApp.Identity.Domain.Exceptions;

[Serializable]
public class InvalidRefreshTokenException : Exception
{
    private static string? DefaultMessage = "Invalid refresh token.";

    public InvalidRefreshTokenException() : base(DefaultMessage) { }
    public InvalidRefreshTokenException(string message)
        : base(message) { }
    public InvalidRefreshTokenException(string message, Exception inner)
        : base(message, inner) { }
}
