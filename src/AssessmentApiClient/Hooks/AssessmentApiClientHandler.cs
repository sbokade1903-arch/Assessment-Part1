using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AssessmentApiClient.Hooks;

internal sealed class AssessmentApiClientHandler(
    IOptions<AssessmentApiClientOptions> options,
    IEnumerable<IAssessmentApiClientHook> hooks,
    ILogger<AssessmentApiClientHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(options.Value.BearerToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Value.BearerToken);
        }

        request.Headers.TryAddWithoutValidation("x-correlation-id", Guid.NewGuid().ToString("N"));

        foreach (var hook in hooks)
        {
            await hook.BeforeRequestAsync(request, cancellationToken);
        }

        var response = await base.SendAsync(request, cancellationToken);
        var body = response.Content is null
            ? string.Empty
            : await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.Content is not null)
        {
            response.Content = new StringContent(body, Encoding.UTF8, response.Content.Headers.ContentType?.MediaType ?? "application/json");
        }

        logger.LogDebug("Assessment API returned {StatusCode} for {Method} {Uri}",
            (int)response.StatusCode,
            request.Method,
            request.RequestUri);

        foreach (var hook in hooks)
        {
            await hook.AfterResponseAsync(response, body, cancellationToken);
        }

        return response;
    }
}
