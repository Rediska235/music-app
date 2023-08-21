using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MusicApp.SongService.Application.Grpc;
using MusicApp.SongService.Application.Grpc.Protos;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Infrastructure.Data;

namespace MusicApp.Tests.SongService.IntegrationTests.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddInMemoryDb(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDbForTesting");
        });

        return services;
    }

    public static IServiceCollection ConfigureGrpc(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(GrpcSongClient));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }

        var mapperMock = new Mock<IMapper>();
        var grpcSongClientMock = new Mock<GrpcSong.GrpcSongClient>();
        var clientMock = new Mock<GrpcSongClient>(mapperMock.Object, grpcSongClientMock.Object);

        clientMock.Setup(mockClient => mockClient.SendSongOperation(It.IsAny<Song>(), It.IsAny<Operation>()));

        services.AddScoped(serviceProvider => clientMock.Object);

        return services;
    }
}