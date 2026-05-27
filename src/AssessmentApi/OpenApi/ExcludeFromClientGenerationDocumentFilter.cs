using System.Reflection;
using AssessmentApi.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AssessmentApi.OpenApi;

/// <summary>
/// Removes endpoints and schemas marked as excluded from the generated OpenAPI document.
/// </summary>
public sealed class ExcludeFromClientGenerationDocumentFilter : IDocumentFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        RemoveExcludedOperations(swaggerDoc, context);
        RemoveExcludedSchemas(swaggerDoc);
    }

    private static void RemoveExcludedOperations(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var excluded = context.ApiDescriptions
            .Where(description =>
                description.ActionDescriptor.EndpointMetadata.OfType<ExcludeFromClientGenerationAttribute>().Any() ||
                description.ActionDescriptor is ControllerActionDescriptor action &&
                action.MethodInfo.GetCustomAttribute<ExcludeFromClientGenerationAttribute>() is not null)
            .ToArray();

        foreach (var description in excluded)
        {
            var path = "/" + description.RelativePath?.TrimEnd('/');
            if (!swaggerDoc.Paths.TryGetValue(path, out var pathItem))
            {
                continue;
            }

            OperationType? operationType = description.HttpMethod?.ToUpperInvariant() switch
            {
                "GET" => OperationType.Get,
                "POST" => OperationType.Post,
                "PUT" => OperationType.Put,
                "PATCH" => OperationType.Patch,
                "DELETE" => OperationType.Delete,
                _ => null
            };

            if (operationType is { } method)
            {
                pathItem.Operations.Remove(method);
            }

            if (pathItem.Operations.Count == 0)
            {
                swaggerDoc.Paths.Remove(path);
            }
        }
    }

    private static void RemoveExcludedSchemas(OpenApiDocument swaggerDoc)
    {
        var excludedSchemaNames = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.GetCustomAttribute<ExcludeModelFromClientGenerationAttribute>() is not null)
            .Select(type => type.Name)
            .ToArray();

        foreach (var schemaName in excludedSchemaNames)
        {
            swaggerDoc.Components?.Schemas?.Remove(schemaName);
        }
    }
}
