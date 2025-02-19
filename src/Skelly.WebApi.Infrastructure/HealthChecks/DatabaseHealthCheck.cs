using Microsoft.Extensions.Diagnostics.HealthChecks;
using Skelly.WebApi.Infrastructure.Persistence;

namespace Skelly.WebApi.Infrastructure.HealthChecks;

public class DatabaseHealthCheck(AppDbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Database.CanConnectAsync(cancellationToken);

            return HealthCheckResult.Healthy("Database is responsive.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database is unavailable.", ex);
        }
    }
}
