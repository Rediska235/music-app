using System.Text.Json.Serialization;

namespace MusicApp.PlaylistService.Domain.Entities;

public class User : Base
{
    public string Username { get; set; } = string.Empty;
    [JsonIgnore]
    public List<Song> Songs { get; set; } = new();
    [JsonIgnore]
    public List<Playlist> Playlists { get; set; } = new();
}
