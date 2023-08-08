using MusicApp.Identity.Application.AutoMapper;
using MusicApp.Identity.Application.Extensions;
using MusicApp.Identity.Infrastructure.Extensions;
using MusicApp.Identity.Web.Extensions;
using MusicApp.Identity.Web.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

Configure.ConfigureLogging();
builder.Host.UseSerilog();

var configuration = builder.Configuration;
builder.Services.AddJwtAuthentication(configuration);
builder.Services.AddCorsPolicy(configuration);
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplication();
builder.Services.AddMassTransitForRabbitMQ();
builder.Services.AddAutoMapper(typeof(UserMapperProfile));

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();
