using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MusicApp.PlaylistService.Web.MessageBroker;
using System.Text;

namespace MusicApp.PlaylistService.Web.Extensions;

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

    public static IServiceCollection AddMassTransitForRabbitMQ(this IServiceCollection services)
    {
        services.AddMassTransit(o =>
        {
            o.SetEndpointNameFormatter(
                new KebabCaseEndpointNameFormatter(prefix: "playlist", includeNamespace: false));

            o.AddConsumers(typeof(UserConsumer).Assembly);

            o.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/");
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
