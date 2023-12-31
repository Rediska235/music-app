﻿namespace MusicApp.PlaylistService.Domain.Entities;

public class Song : Entity
{
    public string Title { get; set; } = string.Empty;
    public User? Artist { get; set; } = new();
    public List<Playlist> Playlists { get; set; } = new();
}
