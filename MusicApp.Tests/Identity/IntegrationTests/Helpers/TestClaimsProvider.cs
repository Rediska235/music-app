using System.Security.Claims;

namespace MusicApp.Tests.Identity.IntegrationTests.Helpers;

public class TestClaimsProvider
{
    public IList<Claim> Claims { get; }

    public TestClaimsProvider()
    {
        Claims = new List<Claim>();
    }

    public static TestClaimsProvider WithUserClaims()
    {
        var provider = new TestClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.Name, "Anton"));
        provider.Claims.Add(new Claim("Role", "artist"));

        return provider;
    }
}