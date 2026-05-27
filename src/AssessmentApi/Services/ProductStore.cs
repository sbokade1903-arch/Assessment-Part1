using AssessmentApi.Models.Common;
using AssessmentApi.Models.Products;

namespace AssessmentApi.Services;

/// <summary>
/// In-memory product data used by the sample API.
/// </summary>
public sealed class ProductStore
{
    private readonly List<ProductResponse> _products =
    [
        new()
        {
            Id = 1,
            Sku = "LAP-15-PRO",
            Name = "15 inch business laptop",
            Price = 92000,
            Inventory = new() { QuantityAvailable = 24, Warehouse = new() { Code = "NGP-01", City = "Nagpur" } }
        },
        new()
        {
            Id = 2,
            Sku = "DOC-STN-USB",
            Name = "USB-C docking station",
            Price = 14500,
            Inventory = new() { QuantityAvailable = 70, Warehouse = new() { Code = "PUN-02", City = "Pune" } }
        },
        new()
        {
            Id = 3,
            Sku = "MON-27-QHD",
            Name = "27 inch QHD monitor",
            Price = 28000,
            Inventory = new() { QuantityAvailable = 18 }
        }
    ];

    /// <summary>
    /// Returns a page of products.
    /// </summary>
    public Task<PagedResult<ProductResponse>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var items = _products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArray();

        return Task.FromResult(new PagedResult<ProductResponse>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = _products.Count,
            Items = items
        });
    }

    /// <summary>
    /// Gets a product by identifier.
    /// </summary>
    public Task<ProductResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = _products.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(product);
    }

    /// <summary>
    /// Creates a product in memory.
    /// </summary>
    public Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = new ProductResponse
        {
            Id = _products.Max(x => x.Id) + 1,
            Sku = request.Sku,
            Name = request.Name,
            Price = request.Price,
            Currency = request.Currency,
            Inventory = new() { QuantityAvailable = request.QuantityAvailable }
        };

        _products.Add(product);
        return Task.FromResult(product);
    }
}
