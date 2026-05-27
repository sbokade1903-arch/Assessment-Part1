namespace AssessmentApi.Attributes;

/// <summary>
/// Removes an endpoint or model from the OpenAPI document used for client generation.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ExcludeFromClientGenerationAttribute : Attribute;
