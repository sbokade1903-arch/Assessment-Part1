using System.ComponentModel.DataAnnotations;

namespace AssessmentApi.Models.Products;

/// <summary>
/// Product details returned by the API.
/// </summary>
public sealed record ProductResponse
{
    /// <summary>
    /// Product identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Stock keeping unit.
    /// </summary>
    public string Sku { get; init; } = string.Empty;

    /// <summary>
    /// Product name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Unit price in the configured currency.
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Currency used for the price.
    /// </summary>
    public string Currency { get; init; } = "INR";

    /// <summary>
    /// Whether the product can be sold.
    /// </summary>
    public bool IsActive { get; init; } = true;

    /// <summary>
    /// Product inventory details.
    /// </summary>
    public InventoryInfo Inventory { get; init; } = new();
}

/// <summary>
/// Request body used to create a product.
/// </summary>
public sealed record CreateProductRequest
{
    /// <summary>
    /// Stock keeping unit.
    /// </summary>
    [Required, StringLength(30, MinimumLength = 3)]
    public string Sku { get; init; } = string.Empty;

    /// <summary>
    /// Product display name.
    /// </summary>
    [Required, StringLength(120, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Unit price.
    /// </summary>
    [Range(0.01, 999999)]
    public decimal Price { get; init; }

    /// <summary>
    /// Price currency.
    /// </summary>
    [Required, StringLength(3, MinimumLength = 3)]
    public string Currency { get; init; } = "INR";

    /// <summary>
    /// Initial available quantity.
    /// </summary>
    [Range(0, 100000)]
    public int QuantityAvailable { get; init; } = 0;
}

/// <summary>
/// Inventory details for a product.
/// </summary>
public sealed record InventoryInfo
{
    /// <summary>
    /// Quantity available to sell.
    /// </summary>
    public int QuantityAvailable { get; init; }

    /// <summary>
    /// Warehouse location.
    /// </summary>
    public WarehouseInfo Warehouse { get; init; } = new();

    /// <summary>
    /// Reorder threshold.
    /// </summary>
    public int ReorderLevel { get; init; } = 10;
}

/// <summary>
/// Warehouse location details.
/// </summary>
public sealed record WarehouseInfo
{
    /// <summary>
    /// Warehouse code.
    /// </summary>
    public string Code { get; init; } = "NGP-01";

    /// <summary>
    /// Warehouse city.
    /// </summary>
    public string City { get; init; } = "Nagpur";
}
