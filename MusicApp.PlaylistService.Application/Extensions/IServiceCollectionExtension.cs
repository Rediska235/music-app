using Microsoft.Extensions.DependencyInjection;
using MusicApp.PlaylistService.Application.Services.Implementations;
using MusicApp.PlaylistService.Application.Services.Interfaces;

namespace MusicApp.PlaylistService.Application.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPlaylistsService, PlaylistsService>();
        services.AddScoped<UserService>();

        return services;
    }
}
