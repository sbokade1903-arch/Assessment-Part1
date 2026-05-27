namespace AssessmentApiClient;

/// <summary>
/// Options used when registering the generated assessment API clients.
/// </summary>
public sealed class AssessmentApiClientOptions
{
    /// <summary>
    /// Base address of the running Assessment API.
    /// </summary>
    public Uri BaseAddress { get; set; } = new("https://localhost:5001");

    /// <summary>
    /// Optional bearer token added to outgoing requests.
    /// </summary>
    public string? BearerToken { get; set; }
}
