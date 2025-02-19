using Microsoft.Extensions.Diagnostics.HealthChecks;
using Skelly.WebApi.Presentation.Responses;

namespace Skelly.WebApi.FunctionalTests.Features.HealthCheck;

public class LivenessHealthCheckTests(PresentationFactory factory) : IClassFixture<PresentationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GivenApplicationIsRunning_WhenGettingLiveness_ThenReturnsOK()
    {
        // When
        var response = await _client.GetAsync("api/v1/health-check/liveness");

        // Then
        var result = await response.Content.ReadFromJsonAsync<HealthCheckResponse>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(result.Status, HealthStatus.Healthy.ToString());
        Assert.Null(result.Dependencies);
    }
}
