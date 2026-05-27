using AssessmentApiClient.Hooks;
using AssessmentApiClient.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AssessmentApiClient;

/// <summary>
/// Dependency injection helpers for Assessment API clients.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers generated Assessment API clients backed by HttpClientFactory.
    /// </summary>
    public static IServiceCollection AddAssessmentApiClient(
        this IServiceCollection services,
        Action<AssessmentApiClientOptions> configureOptions,
        Action<HttpClient>? configureHttpClient = null)
    {
        services.Configure(configureOptions);
        services.AddTransient<AssessmentApiClientHandler>();

        services.AddHttpClient<IAssessmentHttpClient, AssessmentHttpClient>((provider, client) =>
            ConfigureClient(provider, client, configureHttpClient))
            .AddHttpMessageHandler<AssessmentApiClientHandler>();

        return services;
    }

    private static void ConfigureClient(IServiceProvider provider, HttpClient client, Action<HttpClient>? configureHttpClient)
    {
        var options = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<AssessmentApiClientOptions>>().Value;
        client.BaseAddress = options.BaseAddress;
        configureHttpClient?.Invoke(client);
    }
}
