namespace MusicApp.PlaylistService.Domain.Entities;

public class User : Entity
{
    public string Username { get; set; } = string.Empty;
    public List<Song> Songs { get; set; } = new();
    public List<Playlist> Playlists { get; set; } = new();
}
