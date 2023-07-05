using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicApp.PlaylistService.Infrastructure.Data;

namespace MusicApp.PlaylistService.Infrastructure.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        //services.AddScoped<ISongRepository, SongRepository>();
        //services.AddScoped<IArtistRepository, ArtistRepository>();

        return services;
    }
}
