using System.Text.Json.Serialization;
using AssessmentApiClient.Models;

namespace AssessmentApiClient.Serialization;

/// <summary>
/// Source-generated JSON metadata used by the generated API client.
/// </summary>
[JsonSerializable(typeof(ApiErrorResponse))]
[JsonSerializable(typeof(ApiResponseOfOrderResponse))]
[JsonSerializable(typeof(ApiResponseOfCustomerResponse))]
[JsonSerializable(typeof(ApiResponseOfProductResponse))]
[JsonSerializable(typeof(ApiResponseOfPagedResultOfProductResponse))]
[JsonSerializable(typeof(PagedResultOfProductResponse))]
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
[JsonSerializable(typeof(Dictionary<string, ICollection<string>>))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Metadata)]
public sealed partial class AssessmentClientJsonContext : JsonSerializerContext;
