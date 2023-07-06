namespace MusicApp.PlaylistService.Domain.Entities;

public class Song
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid ArtistId { get; set; }
    public User Artist { get; set; } = new();
    public List<Playlist> Playlists { get; set; } = new();
}
