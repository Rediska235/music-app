namespace MusicApp.Identity.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool IsArtist { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public List<Role> Roles { get; set; } = new();
}
