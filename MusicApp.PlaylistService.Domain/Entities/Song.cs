using System.Text.Json.Serialization;

namespace MusicApp.PlaylistService.Domain.Entities;

public class Song
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public User Artist { get; set; } = new();
    [JsonIgnore]
    public List<Playlist> Playlists { get; set; } = new();
}
