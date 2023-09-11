using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using MusicApp.SongService.Web;
using MusicApp.Tests.SongService.IntegrationTests.Extensions;

namespace MusicApp.Tests.SongService.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddInMemoryDb();

            services.ConfigureGrpc();
        });
    }
}