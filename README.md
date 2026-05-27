# Assessment Part 1

Only Part 1 of the assessment is implemented intentionally.

This solution contains a .NET 10 ASP.NET Core Web API and a generated .NET 10 API client. Part 2 items such as Vue, Nuxt, frontend hosts, Tailwind, Module Federation, and Web Components are intentionally not included.

## Projects

```text
D:\Assessment-Part1
├── AssessmentPart1.sln
├── src
│   ├── AssessmentApi
│   └── AssessmentApiClient
└── README.md
```

## Technologies Used

- .NET 10
- ASP.NET Core Web API
- MVC Controllers and Minimal APIs
- Swashbuckle.AspNetCore for Swagger/OpenAPI
- NSwag.MSBuild for build-time C# client generation
- System.Text.Json source generation
- HttpClientFactory and dependency injection

## API Overview

`AssessmentApi` exposes practical mock endpoints for:

- Orders through MVC controllers
- Customers through MVC controllers
- Products through Minimal APIs

The API uses nested DTOs such as `OrderResponse`, `CustomerInfo`, `AddressInfo`, `ProductItem`, `PaymentSummary`, and inventory-related product models. Several DTO properties include explicit default values such as `Priority = 100`, `Currency = "INR"`, and `IsActive = true`.

The API also includes generic contracts such as `ApiResponse<T>` and `PagedResult<T>`, plus a deliberately long-namespaced model:

```csharp
AssessmentApi.Models.Domain.Inventory.Ordering.Advanced.CustomerProfile.CustomerProfile
```

The generated client exposes clean generated model names under `AssessmentApiClient.Models`.

## Error Handling

All API error responses use one strongly typed model: `ApiErrorResponse`.

It includes:

- `traceId`
- `statusCode`
- `message`
- `errors`
- `timestamp`

Controller validation errors, Minimal API validation errors, not-found responses, and unhandled exceptions all return this model.

## JSON Source Generation

Both projects use `System.Text.Json` source generation. The API configures its source-generated context globally through `TypeInfoResolverChain` for MVC controllers and Minimal APIs. Reflection-based JSON serialization is disabled through the project file.

The generated client uses `System.Text.Json` and wires its generated DTO metadata into NSwag's serializer settings through a partial client class.

Source generation was chosen because it makes serialization behavior explicit, improves startup/runtime performance, and catches missing contract metadata earlier during development.

## Swagger and Client Generation

Swagger is provided by Swashbuckle and includes XML comments from the API assembly.

`AssessmentApiClient` regenerates its C# client during `dotnet build`:

1. The API project builds.
2. `Swashbuckle.AspNetCore.Cli` writes `swagger.json` from the built API assembly.
3. `NSwag.MSBuild` generates `Generated\AssessmentApiClient.g.cs`.
4. The generated client is compiled into the class library.

NSwag was chosen because it is mature, build-friendly, supports strongly typed C# clients, preserves OpenAPI/XML documentation well, and works cleanly with `HttpClientFactory`.

## Swagger Exclusion

The API includes:

- `ExcludeFromClientGenerationAttribute` for endpoints
- `ExcludeModelFromClientGenerationAttribute` for models

The Swagger document filter removes marked endpoints and schemas from the generated OpenAPI document.

## Dependency Injection

Register the generated client with:

```csharp
services.AddAssessmentApiClient(options =>
{
    options.BaseAddress = new Uri("https://localhost:5001");
    options.BearerToken = "token";
});
```

The registration supports:

- Custom `HttpClient` configuration
- Auth header injection
- Correlation id injection
- Request and response hooks through `IAssessmentApiClientHook`
- Typed `AssessmentApiException` for non-success responses

A small example is included in:

```text
src\AssessmentApiClient\Samples\DependencyInjectionSample.cs
```

## Run the API

```powershell
cd D:\Assessment-Part1
dotnet run --project .\src\AssessmentApi\AssessmentApi.csproj
```

Open Swagger:

```text
https://localhost:5001/swagger
```

The HTTP profile is also available at:

```text
http://localhost:5000/swagger
```

## Build

```powershell
cd D:\Assessment-Part1
dotnet restore
dotnet build
```

Both projects target `net10.0`.

## Design Decisions

- The implementation uses in-memory stores only, as requested.
- Controllers are used for orders and customers, while products are implemented with Minimal APIs to show both programming models.
- DTOs are kept close to the domain modules rather than split into excessive layers.
- The client project wraps generated code with practical DI, hooks, and typed exception behavior without adding unnecessary framework code.
- Part 2 is intentionally ignored so the solution remains focused on the requested backend and generated-client assessment.
