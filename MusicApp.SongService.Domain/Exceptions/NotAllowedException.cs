﻿namespace MusicApp.SongService.Domain.Exceptions;

[Serializable]
public class NotAllowedException : Exception
{
    public NotAllowedException() { }
    public NotAllowedException(string message)
        : base(message) { }
    public NotAllowedException(string message, Exception inner)
        : base(message, inner) { }
}
