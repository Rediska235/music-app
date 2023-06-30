﻿namespace MusicApp.Identity.BusinessLogic.DTOs;

public class UserRegisterDto
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool IsArtist { get; set; }
}
