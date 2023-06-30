using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MusicApp.Identity.DataAccess.Data;

namespace MusicApp.Identity.DataAccess;

public static class DatabaseSeeder
{
    public async static Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {

        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await RoleInitializer.InitializeAsync(rolesManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<AppDbContext>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
