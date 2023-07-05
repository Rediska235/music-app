using Microsoft.Extensions.DependencyInjection;

namespace MusicApp.PlaylistService.Application.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //services.AddScoped<ISongsService, SongsService>();

        return services;
    }
}
