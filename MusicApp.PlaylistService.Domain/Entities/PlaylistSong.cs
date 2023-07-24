namespace MusicApp.PlaylistService.Domain.Entities;

public class PlaylistSong
{
    public Guid PlaylistId { get; set; }
    public Guid SongId { get; set; }
}
