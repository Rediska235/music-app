using MusicApp.ApiGateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(configuration);

builder.Services.AddJwtAuthentication(configuration);

var app = builder.Build();

await app.UseOcelot();

app.Run();
