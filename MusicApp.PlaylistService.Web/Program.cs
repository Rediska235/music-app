using MusicApp.PlaylistService.Web.Extensions;
using MusicApp.PlaylistService.Web.Middlewares;
using MusicApp.PlaylistService.Infrastructure.Extensions;
using MusicApp.PlaylistService.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var configuration = builder.Configuration;
builder.Services.AddJwtAuthentication(configuration);
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
