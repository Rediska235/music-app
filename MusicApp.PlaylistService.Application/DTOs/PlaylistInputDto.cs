namespace MusicApp.PlaylistService.Application.DTOs;

public class PlaylistInputDto
{
    public string Name { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
}
