using MusicApp.SongService.Web.Extensions;
using MusicApp.SongService.Application.Extensions;
using MusicApp.SongService.Infrastructure.Extensions;
using MusicApp.SongService.Web.Middlewares;

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