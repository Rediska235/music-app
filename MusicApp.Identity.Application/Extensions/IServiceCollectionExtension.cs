using Microsoft.Extensions.DependencyInjection;
using MusicApp.Identity.Application.Services.Implementations;
using MusicApp.Identity.Application.Services.Interfaces;
namespace MusicApp.Identity.Application.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
