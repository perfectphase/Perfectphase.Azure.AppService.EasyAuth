
using System.Text.Json.Serialization;

namespace Perfectphase.Azure.AppService.EasyAuth.Shared;

public class EasyAuthPrincipalModel
{
    [JsonPropertyName("auth_typ")]
    public required string AuthenticationType { get; init; }

    [JsonPropertyName("name_typ")]
    public required string NameClaimType { get; init; }

    [JsonPropertyName("role_typ")]
    public required string RoleClaimType { get; init; }

    [JsonPropertyName("claims")]
    public required ClaimModel[] Claims { get; init; }
}