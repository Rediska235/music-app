using System.ComponentModel.DataAnnotations;

namespace MusicApp.Identity.BusinessLogic.DTOs;

public class UserRegisterDto
{
    [Required]
    public string UserName { get; set; } = String.Empty;

    [Required]
    public string Password { get; set; } = String.Empty;

    public bool IsArtist { get; set; }
}
