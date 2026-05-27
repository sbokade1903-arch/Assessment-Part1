using System.ComponentModel.DataAnnotations;
using AssessmentApi.Models.Customers;

namespace AssessmentApi.Models.Orders;

/// <summary>
/// Order details returned by the API.
/// </summary>
public sealed record OrderResponse
{
    /// <summary>
    /// Unique order identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Customer purchase order reference.
    /// </summary>
    public string OrderNumber { get; init; } = string.Empty;

    /// <summary>
    /// Current order status.
    /// </summary>
    public string Status { get; init; } = "Draft";

    /// <summary>
    /// Order priority where lower values are handled first.
    /// </summary>
    public int Priority { get; init; } = 100;

    /// <summary>
    /// Customer that placed the order.
    /// </summary>
    public CustomerInfo Customer { get; init; } = new();

    /// <summary>
    /// Ordered products.
    /// </summary>
    public IReadOnlyList<ProductItem> ProductItems { get; init; } = [];

    /// <summary>
    /// Payment totals for the order.
    /// </summary>
    public PaymentSummary Payment { get; init; } = new();

    /// <summary>
    /// Requested delivery information.
    /// </summary>
    public DeliveryPlan Delivery { get; init; } = new();
}

/// <summary>
/// Request body used to create an order.
/// </summary>
public sealed record CreateOrderRequest
{
    /// <summary>
    /// Customer placing the order.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int CustomerId { get; init; }

    /// <summary>
    /// Client-provided order reference.
    /// </summary>
    [Required, StringLength(40, MinimumLength = 3)]
    public string OrderNumber { get; init; } = string.Empty;

    /// <summary>
    /// Order priority where lower values are handled first.
    /// </summary>
    [Range(1, 999)]
    public int Priority { get; init; } = 100;

    /// <summary>
    /// Currency used for all order amounts.
    /// </summary>
    [Required, StringLength(3, MinimumLength = 3)]
    public string Currency { get; init; } = "INR";

    /// <summary>
    /// Products included in the order.
    /// </summary>
    [MinLength(1)]
    public IReadOnlyList<CreateOrderItemRequest> Items { get; init; } = [];
}

/// <summary>
/// Product item in a create order request.
/// </summary>
public sealed record CreateOrderItemRequest
{
    /// <summary>
    /// Product identifier.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int ProductId { get; init; }

    /// <summary>
    /// Quantity ordered.
    /// </summary>
    [Range(1, 500)]
    public int Quantity { get; init; } = 1;
}

/// <summary>
/// Product line item in an order response.
/// </summary>
public sealed record ProductItem
{
    /// <summary>
    /// Product identifier.
    /// </summary>
    public int ProductId { get; init; }

    /// <summary>
    /// Product name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Quantity ordered.
    /// </summary>
    public int Quantity { get; init; } = 1;

    /// <summary>
    /// Unit price.
    /// </summary>
    public decimal UnitPrice { get; init; }

    /// <summary>
    /// Line total.
    /// </summary>
    public decimal LineTotal { get; init; }
}

/// <summary>
/// Payment totals for an order.
/// </summary>
public sealed record PaymentSummary
{
    /// <summary>
    /// Transaction currency.
    /// </summary>
    public string Currency { get; init; } = "INR";

    /// <summary>
    /// Subtotal before tax.
    /// </summary>
    public decimal Subtotal { get; init; }

    /// <summary>
    /// Tax amount.
    /// </summary>
    public decimal Tax { get; init; }

    /// <summary>
    /// Total including tax.
    /// </summary>
    public decimal Total { get; init; }

    /// <summary>
    /// Whether payment has been captured.
    /// </summary>
    public bool IsPaid { get; init; }
}

/// <summary>
/// Delivery planning details.
/// </summary>
public sealed record DeliveryPlan
{
    /// <summary>
    /// Preferred carrier.
    /// </summary>
    public string Carrier { get; init; } = "Standard";

    /// <summary>
    /// Estimated delivery date.
    /// </summary>
    public DateOnly EstimatedDate { get; init; } = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));

    /// <summary>
    /// Whether weekend delivery is allowed.
    /// </summary>
    public bool AllowWeekendDelivery { get; init; } = false;
}
