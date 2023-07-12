namespace MusicApp.Identity.Domain.Entities;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}
