namespace MusicApp.PlaylistService.Application.DTOs;

public class SongOutputDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public UserOutputDto Artist { get; set; } = new();
}
