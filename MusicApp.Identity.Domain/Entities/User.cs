using System.ComponentModel.DataAnnotations;

namespace MusicApp.Identity.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    [MaxLength(30)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(60)]
    public string PasswordHash { get; set; } = string.Empty;

    public bool IsArtist { get; set; }

    [MaxLength(300)]
    public string RefreshToken { get; set; } = string.Empty;

    public DateTime TokenCreated { get; set; }

    public DateTime TokenExpires { get; set; }

    public List<Role> Roles { get; set; } = new List<Role>();
}
