namespace AssessmentApi.Models.Domain.Inventory.Ordering.Advanced.CustomerProfile;

/// <summary>
/// Advanced customer profile model kept in a deliberately long namespace.
/// </summary>
public sealed record CustomerProfile
{
    /// <summary>
    /// Profile identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Customer segment name.
    /// </summary>
    public string Segment { get; init; } = "Retail";

    /// <summary>
    /// Whether advanced ordering rules are enabled.
    /// </summary>
    public bool IsActive { get; init; } = true;
}
