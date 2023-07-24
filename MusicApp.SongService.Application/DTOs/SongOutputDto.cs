namespace MusicApp.SongService.Application.DTOs;

public class SongOutputDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ArtistOutputDto Artist { get; set; } = new();
}
