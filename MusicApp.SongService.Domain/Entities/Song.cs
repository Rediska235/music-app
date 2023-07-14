namespace MusicApp.SongService.Domain.Entities;

public class Song
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Artist Artist { get; set; } = new();
}
