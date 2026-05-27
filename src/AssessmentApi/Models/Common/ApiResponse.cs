namespace AssessmentApi.Models.Common;

/// <summary>
/// Standard success wrapper returned by API endpoints.
/// </summary>
/// <typeparam name="T">The response payload type.</typeparam>
public sealed record ApiResponse<T>
{
    /// <summary>
    /// Indicates whether the request completed successfully.
    /// </summary>
    public bool Success { get; init; } = true;

    /// <summary>
    /// Human-readable result message.
    /// </summary>
    public string Message { get; init; } = "Request completed successfully.";

    /// <summary>
    /// Response payload.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Server-side trace identifier for support and diagnostics.
    /// </summary>
    public string TraceId { get; init; } = string.Empty;
}
