using Microsoft.AspNetCore.Identity;

namespace Identity.Models;

public class User : IdentityUser
{
    public bool IsArtist { get; set; }
}
