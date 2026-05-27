using AssessmentApiClient.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AssessmentApiClient.Samples;

/// <summary>
/// Small example showing dependency injection registration and typed exception handling.
/// </summary>
public static class DependencyInjectionSample
{
    /// <summary>
    /// Demonstrates registering and calling the generated products client.
    /// </summary>
    public static async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var services = new ServiceCollection()
            .AddAssessmentApiClient(options =>
            {
                options.BaseAddress = new Uri("https://localhost:5001");
                options.BearerToken = "sample-token";
            });

        await using var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<IAssessmentHttpClient>();

        try
        {
            var response = await client.GetProductByIdAsync(1, cancellationToken);
            Console.WriteLine(response.Data?.Name);
        }
        catch (AssessmentApiException ex)
        {
            Console.WriteLine($"API call failed with {ex.StatusCode}. TraceId: {ex.TraceId}");
        }
    }
}
