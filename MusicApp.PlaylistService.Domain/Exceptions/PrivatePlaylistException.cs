namespace MusicApp.PlaylistService.Domain.Exceptions;

[Serializable]
public class PrivatePlaylistException : Exception
{
    private static string? DefaultMessage = "This playlist is private.";

    public PrivatePlaylistException() : base(DefaultMessage) { }
    public PrivatePlaylistException(string message)
        : base(message) { }
    public PrivatePlaylistException(string message, Exception inner)
        : base(message, inner) { }
}
