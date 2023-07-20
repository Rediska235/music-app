using System.Text.Json.Serialization;

namespace MusicApp.SongService.Domain.Entities;

public class Artist : Base
{
    public string Username { get; set; } = string.Empty;
    [JsonIgnore]
    public List<Song> Songs { get; set; } = new();
}
