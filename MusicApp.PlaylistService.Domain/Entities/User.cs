using System.Text.Json.Serialization;

namespace MusicApp.PlaylistService.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    [JsonIgnore]
    public List<Playlist> Playlists { get; set; } = new();
}
