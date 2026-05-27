using System.ComponentModel.DataAnnotations;
using AssessmentApi.Attributes;
using AssessmentApi.Errors;
using AssessmentApi.Models.Common;
using AssessmentApi.Models.Products;
using AssessmentApi.Services;

namespace AssessmentApi.Endpoints;

/// <summary>
/// Minimal API endpoint mappings for products.
/// </summary>
public static class ProductEndpoints
{
    /// <summary>
    /// Registers product minimal API endpoints.
    /// </summary>
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products")
            .WithTags("Products");

        group.MapGet("/", GetProducts)
            .WithName("GetProducts")
            .WithSummary("Gets a page of products.")
            .Produces<ApiResponse<PagedResult<ProductResponse>>>()
            .Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:int}", GetProductById)
            .WithName("GetProductById")
            .WithSummary("Gets a product by identifier.")
            .Produces<ApiResponse<ProductResponse>>()
            .Produces<ApiErrorResponse>(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateProduct)
            .WithName("CreateProduct")
            .WithSummary("Creates a product.")
            .Produces<ApiResponse<ProductResponse>>(StatusCodes.Status201Created)
            .Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest);

        group.MapGet("/internal/health", () => Results.Ok(new InternalHealthResponse()))
            .WithName("InternalProductsHealth")
            .WithMetadata(new ExcludeFromClientGenerationAttribute());

        return app;
    }

    /// <summary>
    /// Gets a page of products.
    /// </summary>
    public static async Task<IResult> GetProducts(
        int page,
        int pageSize,
        ProductStore products,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        if (page < 1 || pageSize is < 1 or > 100)
        {
            return Results.BadRequest(ApiErrorResponse.Create(
                httpContext,
                StatusCodes.Status400BadRequest,
                "Page must be at least 1 and pageSize must be between 1 and 100."));
        }

        var result = await products.GetPageAsync(page, pageSize, cancellationToken);
        return Results.Ok(new ApiResponse<PagedResult<ProductResponse>>
        {
            Data = result,
            TraceId = httpContext.TraceIdentifier
        });
    }

    /// <summary>
    /// Gets a product by identifier.
    /// </summary>
    public static async Task<IResult> GetProductById(
        int id,
        ProductStore products,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var product = await products.GetByIdAsync(id, cancellationToken);
        return product is null
            ? Results.NotFound(ApiErrorResponse.Create(httpContext, StatusCodes.Status404NotFound, "Product was not found."))
            : Results.Ok(new ApiResponse<ProductResponse> { Data = product, TraceId = httpContext.TraceIdentifier });
    }

    /// <summary>
    /// Creates a product.
    /// </summary>
    public static async Task<IResult> CreateProduct(
        CreateProductRequest request,
        ProductStore products,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var validation = Validate(request);
        if (validation.Count > 0)
        {
            return Results.BadRequest(ApiErrorResponse.Create(
                httpContext,
                StatusCodes.Status400BadRequest,
                "One or more validation errors occurred.",
                validation));
        }

        var product = await products.CreateAsync(request, cancellationToken);
        var response = new ApiResponse<ProductResponse>
        {
            Data = product,
            Message = "Product created.",
            TraceId = httpContext.TraceIdentifier
        };

        return Results.Created($"/products/{product.Id}", response);
    }

    private static Dictionary<string, string[]> Validate<T>(T model)
    {
        var context = new ValidationContext(model!);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model!, context, results, validateAllProperties: true);

        return results
            .SelectMany(result => result.MemberNames.DefaultIfEmpty(string.Empty), (result, member) => new { member, result.ErrorMessage })
            .GroupBy(x => x.member)
            .ToDictionary(
                x => x.Key,
                x => x.Select(y => y.ErrorMessage ?? "Invalid value.").ToArray());
    }
}

/// <summary>
/// Internal health response excluded from generated client contracts.
/// </summary>
[ExcludeModelFromClientGeneration]
public sealed record InternalHealthResponse
{
    /// <summary>
    /// Current health status.
    /// </summary>
    public string Status { get; init; } = "ok";
}
