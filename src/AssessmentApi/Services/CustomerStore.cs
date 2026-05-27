using AssessmentApi.Models.Customers;

namespace AssessmentApi.Services;

/// <summary>
/// In-memory customer data used by the sample API.
/// </summary>
public sealed class CustomerStore
{
    private readonly List<CustomerResponse> _customers =
    [
        new()
        {
            Id = 1,
            Name = "Orange City Retail",
            Email = "orders@orangecityretail.example",
            Address = new() { Line1 = "Civil Lines", City = "Nagpur", State = "Maharashtra", PostalCode = "440001" },
            Profile = new() { Tier = "Gold", PaymentTermDays = 15 }
        },
        new()
        {
            Id = 2,
            Name = "Northwind Traders",
            Email = "procurement@northwind.example",
            Address = new() { Line1 = "88 Park Street", City = "Kolkata", State = "West Bengal", PostalCode = "700016" }
        }
    ];

    /// <summary>
    /// Gets a customer by identifier.
    /// </summary>
    public Task<CustomerResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = _customers.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(customer);
    }
}
