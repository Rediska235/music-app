namespace MusicApp.SongService.Application.DTOs;

public class DelayedSongInputDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime PublishTime { get; set; }
}
