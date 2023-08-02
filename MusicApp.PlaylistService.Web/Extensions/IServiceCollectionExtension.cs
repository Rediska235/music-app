using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MusicApp.PlaylistService.Web.Automapper;
using MusicApp.PlaylistService.Web.Grpc;
using MusicApp.PlaylistService.Web.Grpc.Protos;
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

    public static IServiceCollection AddGrpcService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<GrpcSongClient>();
        services.AddAutoMapper(typeof(GrpcModelsMapperProfile));

        services.AddGrpcClient<GrpcSong.GrpcSongClient>(config =>
        {
            config.Address = new Uri(configuration["GrpcHost"]);
        });

        return services;
    }
}
