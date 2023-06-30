using Microsoft.AspNetCore.Identity;

namespace MusicApp.Identity.DataAccess.Models;

public class User : IdentityUser
{
    public bool IsArtist { get; set; }
}
