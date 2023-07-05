namespace MusicApp.PlaylistService.Domain.Exceptions;

public static class CommonExceptions
{
    public static NotAllowedException notYourPlaylist = new("You can not interact with this playlist.");
    public static NotAllowedException privatePlaylist = new("This playlist is private.");
    public static NotFoundException playlistNotFound = new("Playlist not found.");
    public static NotFoundException songNotFound = new("Song not found.");
}
