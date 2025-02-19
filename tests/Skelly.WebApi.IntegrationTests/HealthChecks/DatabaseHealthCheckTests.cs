using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Skelly.WebApi.Infrastructure.HealthChecks;
using Skelly.WebApi.Infrastructure.Persistence;

namespace Skelly.WebApi.IntegrationTests.HealthChecks;

public class DatabaseHealthCheckTests()
{
    [Fact]
    public async Task GivenDatabaseUp_WhenCheckingHealth_ThenReturnsHealthy()
    {
        // Given
        var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);

        context.Database.EnsureCreated();

        var healthCheck = new DatabaseHealthCheck(context);

        // When
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Then
        Assert.Equal(HealthStatus.Healthy, result.Status);
        Assert.Equal("Database is responsive.", result.Description);
    }

    [Fact]
    public async Task GivenDatabaseDown_WhenCheckingHealth_ThenReturnsHealthy()
    {
        // Given
        var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().Options);

        var healthCheck = new DatabaseHealthCheck(context);

        // When
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Then
        Assert.Equal(HealthStatus.Unhealthy, result.Status);
        Assert.Equal("Database is unavailable.", result.Description);
        Assert.NotNull(result.Exception);
    }
}
