using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using MusicApp.Identity.Web;
using MusicApp.Tests.Identity.IntegrationTests.Extensions;

namespace MusicApp.Tests.Identity.IntegrationTests;

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