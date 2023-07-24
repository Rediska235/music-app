namespace MusicApp.SongService.Domain.Entities;

public class Artist : Entity
{
    public string Username { get; set; } = string.Empty;
    public List<Song> Songs { get; set; } = new();
}
