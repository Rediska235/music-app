using Microsoft.Extensions.DependencyInjection;
using MusicApp.Identity.BusinessLogic.Services;

namespace MusicApp.Identity.BusinessLogic;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
