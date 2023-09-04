using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicApp.SongService.Application.AutoMapper;
using MusicApp.SongService.Application.BehaviourPipelines;
using MusicApp.SongService.Application.CQRS.Commands.CreateSong;
using MusicApp.SongService.Application.Grpc;
using MusicApp.SongService.Application.Grpc.Protos;
using MusicApp.SongService.Application.Services.Implementations;
using MusicApp.SongService.Application.Services.Interfaces;
using System.Data.SqlClient;

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

    public async static Task<IServiceCollection> AddHangfireSupport(this IServiceCollection services, IConfiguration configuration)
    {
        await CreateHangfireDb(configuration);

        var connectionString = string.Format(configuration.GetConnectionString("CustomDb"), "HangfireDb");
        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(connectionString);
        });

        services.AddHangfireServer();
  
        return services;
    }

    private async static Task CreateHangfireDb(IConfiguration configuration)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        var connectionString = string.Format(configuration.GetConnectionString("CustomDb"), "master");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new SqlCommand(
                @"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'HangfireDb') 
                    create database HangfireDb;", connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
