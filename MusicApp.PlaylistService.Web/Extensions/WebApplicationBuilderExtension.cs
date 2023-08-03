using Microsoft.AspNetCore.Server.Kestrel.Core;
using MusicApp.PlaylistService.Web.AutoMapper;
using System.Net;

namespace MusicApp.PlaylistService.Web.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder AddGrpcService(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc();
        builder.Services.AddAutoMapper(typeof(GrpcModelsMapperProfile));

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 80, options =>
            {
                options.Protocols = HttpProtocols.Http1;
            });
            options.Listen(IPAddress.Any, 5005, options =>
            {
                options.Protocols = HttpProtocols.Http2;

            });
        });

        return builder;
    }
}
