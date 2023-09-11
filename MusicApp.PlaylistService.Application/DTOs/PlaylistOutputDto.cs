namespace MusicApp.PlaylistService.Application.DTOs;

public class PlaylistOutputDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
    public UserOutputDto Creator { get; set; } = new();
    public List<SongOutputDto> Songs { get; set; } = new();
}
