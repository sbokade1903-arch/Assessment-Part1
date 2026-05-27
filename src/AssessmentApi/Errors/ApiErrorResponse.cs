namespace AssessmentApi.Errors;

/// <summary>
/// Standard error body returned for every API error response.
/// </summary>
public sealed record ApiErrorResponse
{
    /// <summary>
    /// Server trace identifier for troubleshooting.
    /// </summary>
    public string TraceId { get; init; } = string.Empty;

    /// <summary>
    /// HTTP status code.
    /// </summary>
    public int StatusCode { get; init; }

    /// <summary>
    /// Human-readable error message.
    /// </summary>
    public string Message { get; init; } = "The request could not be completed.";

    /// <summary>
    /// Field-level validation errors, when applicable.
    /// </summary>
    public IReadOnlyDictionary<string, string[]> Errors { get; init; } = new Dictionary<string, string[]>();

    /// <summary>
    /// UTC time when the error was created.
    /// </summary>
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Creates a standard error response from the current request context.
    /// </summary>
    public static ApiErrorResponse Create(
        HttpContext context,
        int statusCode,
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null) =>
        new()
        {
            TraceId = context.TraceIdentifier,
            StatusCode = statusCode,
            Message = message,
            Errors = errors ?? new Dictionary<string, string[]>(),
            Timestamp = DateTimeOffset.UtcNow
        };
}
