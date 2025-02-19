using Microsoft.Extensions.Diagnostics.HealthChecks;
using Skelly.WebApi.Presentation.Responses;

namespace Skelly.WebApi.FunctionalTests.Features.HealthCheck;

public class ReadinessHealthCheckTests(PresentationFactory factory) : IClassFixture<PresentationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GivenApplicationIsRunning_WhenGettingReadiness_ThenReturnsOK()
    {
        // When
        var response = await _client.GetAsync("api/v1/health-check/readiness");

        // Then
        var result = await response.Content.ReadFromJsonAsync<HealthCheckResponse>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(result.Status, HealthStatus.Healthy.ToString());
        Assert.NotNull(result.Dependencies);
        Assert.Single(result.Dependencies);

        var dependency = result.Dependencies.ElementAt(0);
        Assert.Equal("Database", dependency.Name);
        Assert.Equal(HealthStatus.Healthy.ToString(), dependency.Status);
        Assert.NotEqual(default, dependency.Duration);
        Assert.Null(dependency.ErrorMessage);
    }
}