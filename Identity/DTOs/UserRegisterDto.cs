using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs;

public class UserRegisterDto
{
    [Required]
    public string UserName { get; set; } = String.Empty;

    [Required]
    public string Password { get; set; } = String.Empty;

    public bool IsArtist { get; set; }
}
