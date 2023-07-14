namespace MusicApp.SongService.Domain.Exceptions;

[Serializable]
public class NotYourSongException : Exception
{
    private static string? DefaultMessage = "You can not interact with this song.";

    public NotYourSongException() : base(DefaultMessage) { }
    public NotYourSongException(string message)
        : base(message) { }
    public NotYourSongException(string message, Exception inner)
        : base(message, inner) { }
}
