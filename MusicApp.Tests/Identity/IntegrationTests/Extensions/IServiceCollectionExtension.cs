using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MusicApp.Identity.Infrastructure.Data;
using MusicApp.Shared;

namespace MusicApp.Tests.Identity.IntegrationTests.Extensions;

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