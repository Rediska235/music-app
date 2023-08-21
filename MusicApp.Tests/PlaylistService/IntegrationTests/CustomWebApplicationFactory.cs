using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using MusicApp.PlaylistService.Web;
using MusicApp.Tests.PlaylistService.IntegrationTests.Extensions;

namespace MusicApp.Tests.PlaylistService.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddInMemoryDb();
        });
    }
}