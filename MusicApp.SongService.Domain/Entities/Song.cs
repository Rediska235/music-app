namespace MusicApp.SongService.Domain.Entities;

public class Song : Entity
{
    public string Title { get; set; } = string.Empty;
    public Artist Artist { get; set; } = new();
}
