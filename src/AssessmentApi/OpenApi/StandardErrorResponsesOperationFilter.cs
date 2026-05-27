using AssessmentApi.Errors;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AssessmentApi.OpenApi;

/// <summary>
/// Adds the standard API error model to common failure responses.
/// </summary>
public sealed class StandardErrorResponsesOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var schema = context.SchemaGenerator.GenerateSchema(typeof(ApiErrorResponse), context.SchemaRepository);
        var content = new Dictionary<string, OpenApiMediaType>
        {
            ["application/json"] = new() { Schema = schema }
        };

        foreach (var statusCode in new[] { "400", "404", "500" })
        {
            operation.Responses.TryAdd(statusCode, new OpenApiResponse
            {
                Description = statusCode == "500" ? "Server error" : "Client error",
                Content = content
            });
        }
    }
}
