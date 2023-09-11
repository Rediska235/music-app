namespace MusicApp.SongService.Domain.Exceptions;

[Serializable]
public class ArtistNotFoundException : Exception
{
    private static string? DefaultMessage = "Artist not found.";

    public ArtistNotFoundException() : base(DefaultMessage) { }

    public ArtistNotFoundException(string message)
        : base(message) { }

    public ArtistNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}
