﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Infrastructure.Data;
using MusicApp.SongService.Infrastructure.Repositories.MongoDb;
using MusicApp.SongService.Infrastructure.Repositories.SqlServer;

namespace MusicApp.SongService.Infrastructure.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<ISongMongoDbRepository, SongMongoDbRepository>();
        services.AddScoped<ISongRepository, SongRepository>();
        services.AddScoped<IArtistRepository, ArtistRepository>();

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "musicapp_";
        });

        return services;
    }

    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDb"));
        services.AddSingleton(mongoClient);

        var db = mongoClient.GetDatabase("SongServiceMongoDb");
        db.CreateCollection("Songs");

        return services;
    }
}
