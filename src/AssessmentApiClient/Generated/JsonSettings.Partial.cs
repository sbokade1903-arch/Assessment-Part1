using AssessmentApiClient.Serialization;

namespace AssessmentApiClient.Models;

public partial class AssessmentHttpClient
{
    static partial void UpdateJsonSerializerSettings(System.Text.Json.JsonSerializerOptions settings)
    {
        settings.TypeInfoResolverChain.Insert(0, AssessmentClientJsonContext.Default);
    }
}
