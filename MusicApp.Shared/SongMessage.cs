namespace MusicApp.Shared;

public class SongMessage
{
    public SongPublishedDto Song { get; set; } = new();
    public Operation Operation { get; set; }
}
