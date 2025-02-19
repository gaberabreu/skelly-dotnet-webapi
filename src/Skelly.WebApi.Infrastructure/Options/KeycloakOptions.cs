namespace Skelly.WebApi.Infrastructure.Options;

public class KeycloakOptions
{
    public required string AuthServerUrl { get; init; }
    public required string Realm { get; init; }
    public required string ClientId { get; init; }
    public string? ClientSecret { get; init; }

    public string Authority => $"{AuthServerUrl}/realms/{Realm}";
    public string AuthorizationUrl => $"{AuthServerUrl}/realms/{Realm}/protocol/openid-connect/auth";
    public string TokenUrl => $"{AuthServerUrl}/realms/{Realm}/protocol/openid-connect/token";
}
