namespace MusicApp.PlaylistService.Domain.Exceptions;

[Serializable]
public class PlaylistNotFoundException : Exception
{
    private static string? DefaultMessage = "Playlist not found.";

    public PlaylistNotFoundException() : base(DefaultMessage) { }

    public PlaylistNotFoundException(string message)
        : base(message) { }

    public PlaylistNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}
