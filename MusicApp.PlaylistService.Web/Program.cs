using MusicApp.PlaylistService.Application.Extensions;
using MusicApp.PlaylistService.Infrastructure.Extensions;
using MusicApp.PlaylistService.Web.Extensions;
using MusicApp.PlaylistService.Web.Grpc;
using MusicApp.PlaylistService.Web.Middlewares;
using MusicApp.PlaylistService.Infrastructure.Extensions;
using MusicApp.PlaylistService.Application.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

Configure.ConfigureLogging();
builder.Host.UseSerilog();

var configuration = builder.Configuration;
builder.Services.AddJwtAuthentication(configuration);
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplication(); 
builder.AddGrpcService();
builder.Services.AddMassTransitForRabbitMQ();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<GrpcSongService>();

app.Run();
