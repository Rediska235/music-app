using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicApp.PlaylistService.Infrastructure.Data;

namespace MusicApp.Tests.PlaylistService.IntegrationTests.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddInMemoryDb(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDbForTesting");
        });

        return services;
    }
}