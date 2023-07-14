using Microsoft.Extensions.DependencyInjection;
using MusicApp.SongService.Application.CQRS;
using MusicApp.SongService.Application.Services;

namespace MusicApp.SongService.Application.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AssemblyReference>());
        services.AddScoped<ArtistService>();

        return services;
    }
}
