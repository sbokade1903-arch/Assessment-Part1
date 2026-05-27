namespace AssessmentApi.Models.Common;

/// <summary>
/// Generic page wrapper used for list responses.
/// </summary>
/// <typeparam name="T">The item type in the page.</typeparam>
public sealed record PagedResult<T>
{
    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; init; } = 1;

    /// <summary>
    /// Maximum items returned in a page.
    /// </summary>
    public int PageSize { get; init; } = 20;

    /// <summary>
    /// Total matching item count.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Items in the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; init; } = [];
}
