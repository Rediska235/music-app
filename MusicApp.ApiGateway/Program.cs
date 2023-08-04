using MusicApp.ApiGateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile($"ocelot.{environment}.json", optional: false, reloadOnChange: true);

var configuration = builder.Configuration;
builder.Services.AddOcelot(configuration);
builder.Services.AddJwtAuthentication(configuration);

var app = builder.Build();

await app.UseOcelot();

app.Run();
