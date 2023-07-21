using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MusicApp.SongService.Application.AutoMapper;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.Services;

namespace MusicApp.SongService.Application.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<CreateSongCommand>());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddValidatorsFromAssembly(typeof(CreateSongCommandValidator).Assembly);

        services.AddAutoMapper(typeof(SongMapperProfile));

        services.AddScoped<ArtistService>();

        return services;
    }
}
