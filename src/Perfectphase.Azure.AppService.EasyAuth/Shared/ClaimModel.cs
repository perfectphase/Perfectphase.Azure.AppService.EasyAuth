using System.Text.Json.Serialization;

namespace Perfectphase.Azure.AppService.EasyAuth.Shared;

public class ClaimModel
{
    [JsonPropertyName("typ")]
    public required string Type { get; init; }

    [JsonPropertyName("val")]
    public required string Value { get; init; }
}
