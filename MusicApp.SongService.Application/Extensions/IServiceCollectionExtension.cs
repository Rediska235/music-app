using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicApp.SongService.Application.AutoMapper;
using MusicApp.SongService.Application.BehaviourPipelines;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.Services;
using MusicApp.SongService.Application.Grpc;
using Microsoft.Extensions.Configuration;
using MusicApp.SongService.Application.Grpc.Protos;
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

    public static IServiceCollection AddHangfireSupport(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HangfireDb");

        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(connectionString);
        });

        services.AddHangfireServer();
  
        return services;
    }
  
    public static IServiceCollection AddGrpcService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(GrpcModelsMapperProfile));

        services.AddGrpcClient<GrpcSong.GrpcSongClient>(config =>
        {
            config.Address = new Uri(configuration["GrpcHost"]);
        });

        services.AddScoped<GrpcSongClient>();

        return services;
    }
}
