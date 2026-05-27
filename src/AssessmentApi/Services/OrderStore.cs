using AssessmentApi.Models.Customers;
using AssessmentApi.Models.Orders;

namespace AssessmentApi.Services;

/// <summary>
/// In-memory order data used by the sample API.
/// </summary>
public sealed class OrderStore(CustomerStore customers, ProductStore products)
{
    private readonly List<OrderResponse> _orders =
    [
        new()
        {
            Id = 1001,
            OrderNumber = "PO-2026-1001",
            Status = "Confirmed",
            Priority = 50,
            Customer = new()
            {
                Id = 1,
                Name = "Orange City Retail",
                Email = "orders@orangecityretail.example",
                Address = new() { Line1 = "Civil Lines", City = "Nagpur", State = "Maharashtra", PostalCode = "440001" }
            },
            ProductItems =
            [
                new() { ProductId = 1, Name = "15 inch business laptop", Quantity = 2, UnitPrice = 92000, LineTotal = 184000 },
                new() { ProductId = 2, Name = "USB-C docking station", Quantity = 2, UnitPrice = 14500, LineTotal = 29000 }
            ],
            Payment = new() { Subtotal = 213000, Tax = 38340, Total = 251340, IsPaid = true },
            Delivery = new() { Carrier = "Blue Dart", EstimatedDate = new DateOnly(2026, 06, 02) }
        }
    ];

    /// <summary>
    /// Gets an order by identifier.
    /// </summary>
    public Task<OrderResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = _orders.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(order);
    }

    /// <summary>
    /// Creates an order in memory.
    /// </summary>
    public async Task<OrderResponse?> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await customers.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        var lines = new List<ProductItem>();
        foreach (var item in request.Items)
        {
            var product = await products.GetByIdAsync(item.ProductId, cancellationToken);
            if (product is null)
            {
                return null;
            }

            lines.Add(new ProductItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                LineTotal = product.Price * item.Quantity
            });
        }

        var subtotal = lines.Sum(x => x.LineTotal);
        var tax = Math.Round(subtotal * 0.18m, 2);

        var order = new OrderResponse
        {
            Id = _orders.Max(x => x.Id) + 1,
            OrderNumber = request.OrderNumber,
            Status = "Confirmed",
            Priority = request.Priority,
            Customer = new CustomerInfo
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Address = customer.Address
            },
            ProductItems = lines,
            Payment = new()
            {
                Currency = request.Currency,
                Subtotal = subtotal,
                Tax = tax,
                Total = subtotal + tax
            }
        };

        _orders.Add(order);
        return order;
    }
}
