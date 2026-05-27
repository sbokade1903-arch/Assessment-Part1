namespace AssessmentApi.Attributes;

/// <summary>
/// Marks a schema as internal so generated clients do not expose it.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class ExcludeModelFromClientGenerationAttribute : Attribute;
