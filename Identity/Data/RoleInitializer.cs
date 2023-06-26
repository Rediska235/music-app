using Microsoft.AspNetCore.Identity;

namespace Identity.Data;

public class RoleInitializer
{
    public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.FindByNameAsync("artist") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("artist"));
        }
    }
}