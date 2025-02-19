using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Skelly.WebApi.Infrastructure.Options;

namespace Skelly.WebApi.Infrastructure.HealthChecks;

public class KeycloakHealthCheck(IHttpClientFactory httpClientFactory, IOptions<KeycloakOptions> keycloakOptions) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(keycloakOptions.Value.Authority, cancellationToken);

            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("Keycloak is responsive.")
                : HealthCheckResult.Degraded("Keycloak is reachable but not fully operational.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Keycloak is unavailable.", ex);
        }
    }
}
