namespace AssessmentApiClient.Hooks;

/// <summary>
/// Hook that can inspect or modify outgoing requests and incoming responses.
/// </summary>
public interface IAssessmentApiClientHook
{
    /// <summary>
    /// Runs immediately before an HTTP request is sent.
    /// </summary>
    ValueTask BeforeRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken);

    /// <summary>
    /// Runs after an HTTP response is received.
    /// </summary>
    ValueTask AfterResponseAsync(HttpResponseMessage response, string responseBody, CancellationToken cancellationToken);
}
