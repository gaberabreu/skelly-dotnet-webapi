using System.Net;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Skelly.WebApi.Infrastructure.HealthChecks;
using Skelly.WebApi.Infrastructure.Options;

namespace Skelly.WebApi.IntegrationTests.HealthChecks;

public class KeycloakHealthCheckTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactory = new();
    private readonly IOptions<KeycloakOptions> _keycloakOptions;

    public KeycloakHealthCheckTests()
    {
        _keycloakOptions = Options.Create(new KeycloakOptions
        {
            AuthServerUrl = "https://fake-keycloak.com/auth",
            Realm = "fake-realm",
            ClientId = "fake-client"
        });
    }

    [Fact]
    public async Task GivenKeycloakUp_WhenCheckingHealth_ThenReturnsHealthy()
    {
        // Given
        SetupHttpClient(HttpStatusCode.OK);

        var healthCheck = new KeycloakHealthCheck(_httpClientFactory.Object, _keycloakOptions);

        // When
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Then
        Assert.Equal(HealthStatus.Healthy, result.Status);
        Assert.Equal("Keycloak is responsive.", result.Description);
    }

    [Fact]
    public async Task GivenKeycloakDegraded_WhenCheckingHealth_ThenReturnsDegraded()
    {
        // Given
        SetupHttpClient(HttpStatusCode.ServiceUnavailable);

        var healthCheck = new KeycloakHealthCheck(_httpClientFactory.Object, _keycloakOptions);

        // When
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Then
        Assert.Equal(HealthStatus.Degraded, result.Status);
        Assert.Equal("Keycloak is reachable but not fully operational.", result.Description);
    }

    [Fact]
    public async Task GivenKeycloakDown_WhenCheckingHealth_ThenReturnsUnhealthy()
    {
        // Given
        _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
            .Throws(new HttpRequestException("Connection failed"));

        var healthCheck = new KeycloakHealthCheck(_httpClientFactory.Object, _keycloakOptions);

        // When
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Then
        Assert.Equal(HealthStatus.Unhealthy, result.Status);
        Assert.Equal("Keycloak is unavailable.", result.Description);
        Assert.NotNull(result.Exception);
    }

    private void SetupHttpClient(HttpStatusCode statusCode)
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode
            });

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
    }
}
