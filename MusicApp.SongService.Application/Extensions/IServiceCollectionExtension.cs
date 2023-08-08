using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MusicApp.SongService.Application.AutoMapper;
using MusicApp.SongService.Application.BehaviourPipelines;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.Services.Implementations;
using MusicApp.SongService.Application.Services.Interfaces;

namespace MusicApp.SongService.Application.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<CreateSongCommand>());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(CreateSongCommandValidator).Assembly);

        services.AddAutoMapper(typeof(SongMapperProfile));

        services.AddScoped<IArtistService, ArtistService>();

        return services;
    }
}
