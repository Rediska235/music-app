using MusicApp.PlaylistService.Application.Extensions;
using MusicApp.PlaylistService.Infrastructure.Extensions;
using MusicApp.PlaylistService.Web.Extensions;
using MusicApp.PlaylistService.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();


var configuration = builder.Configuration;
builder.Services.AddJwtAuthentication(configuration);
builder.Services.AddGrpcService(configuration);
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
