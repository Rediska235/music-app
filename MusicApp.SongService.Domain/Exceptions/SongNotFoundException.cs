namespace MusicApp.SongService.Domain.Exceptions;

[Serializable]
public class SongNotFoundException : Exception
{
    private static string? DefaultMessage = "Song not found.";

    public SongNotFoundException() : base(DefaultMessage) { }

    public SongNotFoundException(string message)
        : base(message) { }

    public SongNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}
