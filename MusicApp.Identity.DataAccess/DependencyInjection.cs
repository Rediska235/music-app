using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicApp.Identity.DataAccess.Data;
using MusicApp.Identity.DataAccess.Models;

namespace MusicApp.Identity.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
}
