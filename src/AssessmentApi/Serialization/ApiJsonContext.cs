using System.Text.Json.Serialization;
using AssessmentApi.Errors;
using AssessmentApi.Models.Common;
using AssessmentApi.Models.Customers;
using AssessmentApi.Models.Domain.Inventory.Ordering.Advanced.CustomerProfile;
using AssessmentApi.Models.Orders;
using AssessmentApi.Models.Products;

namespace AssessmentApi.Serialization;

/// <summary>
/// Source-generated JSON metadata used by ASP.NET Core for request and response serialization.
/// </summary>
[JsonSerializable(typeof(ApiErrorResponse))]
[JsonSerializable(typeof(ApiResponse<OrderResponse>))]
[JsonSerializable(typeof(ApiResponse<CustomerResponse>))]
[JsonSerializable(typeof(ApiResponse<ProductResponse>))]
[JsonSerializable(typeof(ApiResponse<PagedResult<ProductResponse>>))]
[JsonSerializable(typeof(PagedResult<ProductResponse>))]
[JsonSerializable(typeof(OrderResponse))]
[JsonSerializable(typeof(CreateOrderRequest))]
[JsonSerializable(typeof(CreateOrderItemRequest))]
[JsonSerializable(typeof(CustomerResponse))]
[JsonSerializable(typeof(CustomerInfo))]
[JsonSerializable(typeof(AddressInfo))]
[JsonSerializable(typeof(CustomerProfileInfo))]
[JsonSerializable(typeof(CustomerProfile))]
[JsonSerializable(typeof(ProductResponse))]
[JsonSerializable(typeof(CreateProductRequest))]
[JsonSerializable(typeof(ProductItem))]
[JsonSerializable(typeof(PaymentSummary))]
[JsonSerializable(typeof(DeliveryPlan))]
[JsonSerializable(typeof(InventoryInfo))]
[JsonSerializable(typeof(WarehouseInfo))]
[JsonSerializable(typeof(Dictionary<string, string[]>))]
[JsonSerializable(typeof(string[]))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Metadata)]
public sealed partial class ApiJsonContext : JsonSerializerContext;
