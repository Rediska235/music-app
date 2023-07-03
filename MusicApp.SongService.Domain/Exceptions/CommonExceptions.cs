namespace MusicApp.SongService.Domain.Exceptions;

public static class CommonExceptions
{
    public static NotAllowedException notYourSong = new("You can not interact with this song.");
    public static NotFoundException songNotFound = new("Song not found.");
}
