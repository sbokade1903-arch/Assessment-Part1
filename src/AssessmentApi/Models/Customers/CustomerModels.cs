using System.ComponentModel.DataAnnotations;

namespace AssessmentApi.Models.Customers;

/// <summary>
/// Customer details returned by the API.
/// </summary>
public sealed record CustomerResponse
{
    /// <summary>
    /// Unique customer identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Customer display name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Customer email address.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Whether the customer can place new orders.
    /// </summary>
    public bool IsActive { get; init; } = true;

    /// <summary>
    /// Customer billing address.
    /// </summary>
    public AddressInfo Address { get; init; } = new();

    /// <summary>
    /// Commercial profile for the customer.
    /// </summary>
    public CustomerProfileInfo Profile { get; init; } = new();

    /// <summary>
    /// Advanced profile model from a long domain namespace.
    /// </summary>
    public Domain.Inventory.Ordering.Advanced.CustomerProfile.CustomerProfile AdvancedProfile { get; init; } = new();
}

/// <summary>
/// Customer information embedded in order responses.
/// </summary>
public sealed record CustomerInfo
{
    /// <summary>
    /// Customer identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Customer name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Email used for order communication.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Shipping address.
    /// </summary>
    public AddressInfo Address { get; init; } = new();
}

/// <summary>
/// Postal address details.
/// </summary>
public sealed record AddressInfo
{
    /// <summary>
    /// Address line one.
    /// </summary>
    public string Line1 { get; init; } = string.Empty;

    /// <summary>
    /// City name.
    /// </summary>
    public string City { get; init; } = string.Empty;

    /// <summary>
    /// State or province.
    /// </summary>
    public string State { get; init; } = string.Empty;

    /// <summary>
    /// Postal code.
    /// </summary>
    public string PostalCode { get; init; } = string.Empty;

    /// <summary>
    /// Country code.
    /// </summary>
    public string Country { get; init; } = "IN";
}

/// <summary>
/// Request body used to create a new customer.
/// </summary>
public sealed record CreateCustomerRequest
{
    /// <summary>
    /// Customer legal or display name.
    /// </summary>
    [Required, StringLength(120, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Customer email address.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; init; } = string.Empty;
}

/// <summary>
/// Customer commercial settings.
/// </summary>
public sealed record CustomerProfileInfo
{
    /// <summary>
    /// Loyalty tier used for pricing rules.
    /// </summary>
    public string Tier { get; init; } = "Standard";

    /// <summary>
    /// Default payment term in days.
    /// </summary>
    public int PaymentTermDays { get; init; } = 30;

    /// <summary>
    /// Preferred invoice currency.
    /// </summary>
    public string PreferredCurrency { get; init; } = "INR";
}
