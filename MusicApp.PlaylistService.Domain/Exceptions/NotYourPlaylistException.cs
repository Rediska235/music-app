namespace MusicApp.PlaylistService.Domain.Exceptions;

[Serializable]
public class NotYourPlaylistException : Exception
{
    private static string? DefaultMessage = "You can not interact with this playlist.";

    public NotYourPlaylistException() : base(DefaultMessage) { }
    public NotYourPlaylistException(string message)
        : base(message) { }
    public NotYourPlaylistException(string message, Exception inner)
        : base(message, inner) { }
}
