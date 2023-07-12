namespace MusicApp.Identity.Domain.Exceptions;

[Serializable]
public class UsernameIsTakenException : Exception
{
    private static string? DefaultMessage = "This username is taken.";
    
    public UsernameIsTakenException() : base(DefaultMessage) { }
    public UsernameIsTakenException(string message)
        : base(message) { }
    public UsernameIsTakenException(string message, Exception inner)
        : base(message, inner) { }
}
