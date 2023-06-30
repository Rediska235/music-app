using System.ComponentModel.DataAnnotations;

namespace MusicApp.Identity.BusinessLogic.DTOs;

public class UserLoginDto
{
    //TODO fluent validation
    [Required]
    public string UserName { get; set; } = String.Empty;

    [Required]
    public string Password { get; set; } = String.Empty;
}
