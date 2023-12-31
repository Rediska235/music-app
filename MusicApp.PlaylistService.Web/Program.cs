using MusicApp.PlaylistService.Application.Extensions;
using MusicApp.PlaylistService.Infrastructure.Extensions;
using MusicApp.PlaylistService.Web.Extensions;
using MusicApp.PlaylistService.Web.Grpc;
using MusicApp.PlaylistService.Web.Middlewares;
using Serilog;

namespace MusicApp.PlaylistService.Web;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();

        Configure.ConfigureLogging();
        builder.Host.UseSerilog();

        var configuration = builder.Configuration;
        builder.Services.AddJwtAuthentication(configuration);
        builder.Services.AddMassTransitForRabbitMQ(configuration);
        builder.Services.AddRedis(configuration);
        builder.Services.AddInfrastructure(configuration);
        builder.Services.AddApplication();
        builder.AddGrpcService();

        var app = builder.Build();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapGrpcService<GrpcSongService>();

        app.Run();
    }
}