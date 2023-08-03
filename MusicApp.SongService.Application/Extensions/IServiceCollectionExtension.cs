using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MusicApp.SongService.Application.AutoMapper;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.Services;
using MusicApp.SongService.Application.Grpc;
using Microsoft.Extensions.Configuration;
using MusicApp.SongService.Application.Grpc.Protos;

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

    public static IServiceCollection AddGrpcService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<GrpcSongClient>();
        services.AddAutoMapper(typeof(GrpcModelsMapperProfile));

        services.AddGrpcClient<GrpcSong.GrpcSongClient>(config =>
        {
            config.Address = new Uri(configuration["GrpcHost"]);
        });

        return services;
    }
}
