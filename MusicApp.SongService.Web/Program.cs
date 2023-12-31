using Hangfire;
using MusicApp.SongService.Application.Extensions;
using MusicApp.SongService.Infrastructure.Extensions;
using MusicApp.SongService.Web.Extensions;
using MusicApp.SongService.Web.Hangfire;
using MusicApp.SongService.Web.Hubs;
using MusicApp.SongService.Web.Middlewares;
using Serilog;

namespace MusicApp.SongService.Web;

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
        await builder.Services.AddHangfireSupport(configuration);
        builder.Services.AddGrpcService(configuration);
        builder.Services.AddCorsPolicy(configuration);
        builder.Services.AddRedis(configuration);
        builder.Services.AddMongoDb(configuration);
        builder.Services.AddInfrastructure(configuration);
        builder.Services.AddApplication();
        builder.Services.AddSignalR();

        var app = builder.Build();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        var options = new DashboardOptions()
        {
            Authorization = new[] { new HangfireAuthorizationFilter() }
        };
        app.UseHangfireDashboard("/hangfire", options);

        app.UseCors("CorsPolicy");
        app.MapHub<SongHub>("/song");

        app.Run();
    }
}