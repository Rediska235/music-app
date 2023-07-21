namespace MusicApp.PlaylistService.Domain.Entities;

public class Playlist : Entity
{
    public string Name { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
    public User Creator { get; set; } = new();
    public List<Song> Songs { get; set; } = new();
}
