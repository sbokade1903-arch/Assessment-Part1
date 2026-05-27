using AssessmentApiClient.Models;

namespace AssessmentApiClient;

/// <summary>
/// Exception thrown when the Assessment API returns a non-success response.
/// </summary>
public class AssessmentApiException : Exception
{
    /// <summary>
    /// Creates an exception without a typed error payload.
    /// </summary>
    public AssessmentApiException(string message, int statusCode, string? response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception? innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        RawResponseBody = response ?? string.Empty;
        Headers = headers;
    }

    /// <summary>
    /// HTTP status code returned by the API.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Raw response body returned by the API.
    /// </summary>
    public string RawResponseBody { get; }

    /// <summary>
    /// Response headers returned by the API.
    /// </summary>
    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

    /// <summary>
    /// API trace identifier, when returned by the server.
    /// </summary>
    public virtual string? TraceId => null;

    /// <summary>
    /// Validation errors returned by the server, when available.
    /// </summary>
    public virtual IDictionary<string, ICollection<string>>? ValidationErrors => null;
}

/// <summary>
/// Exception thrown when the API returns a typed error response.
/// </summary>
public sealed class AssessmentApiException<TResult> : AssessmentApiException
{
    /// <summary>
    /// Creates an exception with a typed error payload.
    /// </summary>
    public AssessmentApiException(string message, int statusCode, string? response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, Exception? innerException)
        : base(message, statusCode, response, headers, innerException)
    {
        Result = result;
    }

    /// <summary>
    /// Typed response body returned by the API.
    /// </summary>
    public TResult Result { get; }

    /// <inheritdoc />
    public override string? TraceId => Result is ApiErrorResponse error ? error.TraceId : null;

    /// <inheritdoc />
    public override IDictionary<string, ICollection<string>>? ValidationErrors =>
        Result is ApiErrorResponse error ? error.Errors : null;
}
