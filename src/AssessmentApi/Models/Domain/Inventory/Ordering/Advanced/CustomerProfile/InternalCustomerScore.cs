using AssessmentApi.Attributes;

namespace AssessmentApi.Models.Domain.Inventory.Ordering.Advanced.CustomerProfile;

/// <summary>
/// Internal scoring model intentionally excluded from client generation.
/// </summary>
[ExcludeModelFromClientGeneration]
public sealed record InternalCustomerScore
{
    /// <summary>
    /// Internal customer identifier.
    /// </summary>
    public int CustomerId { get; init; }

    /// <summary>
    /// Private risk score.
    /// </summary>
    public int Score { get; init; } = 50;
}
