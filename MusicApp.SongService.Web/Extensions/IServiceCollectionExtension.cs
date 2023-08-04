using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MusicApp.SongService.Web.MessageBroker;
using System.Text;

namespace MusicApp.SongService.Web.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Key").Value);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder.WithOrigins(configuration["SignalRClientHost"])
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed((host) => true));
        });

        return services;
    }
          
    public static IServiceCollection AddMassTransitForRabbitMQ(this IServiceCollection services)
    {
        services.AddMassTransit(config =>
        {
            config.SetEndpointNameFormatter(
                new KebabCaseEndpointNameFormatter(prefix: "song", includeNamespace: false));

            config.AddConsumers(typeof(UserConsumer).Assembly);

            config.UsingRabbitMq((context, config) =>
            {
                config.Host("rabbitmq", "/");
                config.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
