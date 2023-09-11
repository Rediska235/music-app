using System.Text.Json.Serialization;

namespace MusicApp.Identity.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    [JsonIgnore]
    public List<User> Users { get; set; } = new();
}
