using Microsoft.Extensions.DependencyInjection;
using MusicApp.SongService.Application.CQRS;
using MusicApp.SongService.Application.Services.Implementations;
using MusicApp.SongService.Application.Services.Interfaces;

namespace MusicApp.SongService.Application.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISongsService, SongsService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AssemblyReference>());

        return services;
    }
}
