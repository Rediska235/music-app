using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs;

public class UserLoginDto
{
    [Required]
    public string UserName { get; set; } = String.Empty;

    [Required]
    public string Password { get; set; } = String.Empty;
}
