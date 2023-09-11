﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MusicApp.Tests.PlaylistService.IntegrationTests.Helpers;
using System.Net.Http.Headers;

namespace MusicApp.Tests.PlaylistService.IntegrationTests.Extensions;

public static class WebApplicationFactoryExtension
{
    public static HttpClient CreateClientWithTestAuth<T>(this WebApplicationFactory<T> factory, TestClaimsProvider claimsProvider) where T : class
    {
        var client = factory.WithAuthentication(claimsProvider).CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        return client;
    }

    public static WebApplicationFactory<T> WithAuthentication<T>(this WebApplicationFactory<T> factory, TestClaimsProvider claimsProvider) where T : class
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", op => { });

                services.AddScoped(_ => claimsProvider);
            });
        });
    }
}