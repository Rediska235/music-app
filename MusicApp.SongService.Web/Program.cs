using MusicApp.SongService.Application.Extensions;
using MusicApp.SongService.Infrastructure.Extensions;
using MusicApp.SongService.Web.Extensions;
using MusicApp.SongService.Web.Hubs;
using MusicApp.SongService.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

var configuration = builder.Configuration;
builder.Services.AddJwtAuthentication(configuration);
builder.Services.AddCorsPolicy(configuration);
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplication();
builder.Services.AddSignalR();
builder.Services.AddMassTransitForRabbitMQ();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseCors("CorsPolicy");
app.MapHub<SongHub>("/song");

app.Run();