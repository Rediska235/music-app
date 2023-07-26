namespace MusicApp.Shared;

public class SongPublishedDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public UserPublishedDto Artist { get; set; } = new();
}
