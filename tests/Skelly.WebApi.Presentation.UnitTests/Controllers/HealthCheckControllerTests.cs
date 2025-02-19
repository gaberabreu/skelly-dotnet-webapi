using Skelly.WebApi.Presentation.Controllers;
using Skelly.WebApi.Presentation.Responses;

namespace Skelly.WebApi.Presentation.UnitTests.Controllers;

public class HealthCheckControllerTests
{
    private readonly Mock<HealthCheckService> _healthCheckService = new();
    private readonly HealthCheckController _controller;

    public HealthCheckControllerTests()
    {
        _controller = new(_healthCheckService.Object);
    }

    [Fact]
    public async Task GivenHealthyHealthReport_WhenGettingLiveness_ThenReturnsOk()
    {
        // Given
        var healthReport = new HealthReport(new Dictionary<string, HealthReportEntry>(), HealthStatus.Healthy, TimeSpan.Zero);
        _healthCheckService.Setup(s => s.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(healthReport);

        // When
        var result = await _controller.GetLiveness();

        // Then
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<HealthCheckResponse>(objectResult.Value);
        Assert.Equal(HealthStatus.Healthy.ToString(), response.Status);
    }


    [Fact]
    public async Task GivenUnhealthyHealthReport_WhenGettingLiveness_ThenReturnsServiceUnavailable()
    {
        // Given
        var healthReport = new HealthReport(new Dictionary<string, HealthReportEntry>(), HealthStatus.Unhealthy, TimeSpan.Zero);
        _healthCheckService.Setup(s => s.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(healthReport);

        // When
        var result = await _controller.GetLiveness();

        // Then
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, objectResult.StatusCode);
        var response = Assert.IsType<HealthCheckResponse>(objectResult.Value);
        Assert.Equal(HealthStatus.Unhealthy.ToString(), response.Status);
    }

    [Fact]
    public async Task GivenHealthyHealthReport_WhenGettingReadiness_ThenReturnsOk()
    {
        // Given
        var healthReport = new HealthReport(new Dictionary<string, HealthReportEntry>(), HealthStatus.Healthy, TimeSpan.Zero);
        _healthCheckService.Setup(s => s.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(healthReport);

        // When
        var result = await _controller.GetReadiness();

        // Then
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<HealthCheckResponse>(objectResult.Value);
        Assert.Equal(HealthStatus.Healthy.ToString(), response.Status);
    }

    [Fact]
    public async Task GivenUnhealthyHealthReport_WhenGettingReadiness_ThenReturnsServiceUnavailable()
    {
        // Given
        var healthReport = new HealthReport(new Dictionary<string, HealthReportEntry>(), HealthStatus.Unhealthy, TimeSpan.Zero);
        _healthCheckService.Setup(s => s.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(healthReport);

        // When
        var result = await _controller.GetReadiness();

        // Then
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, objectResult.StatusCode);
        var response = Assert.IsType<HealthCheckResponse>(objectResult.Value);
        Assert.Equal(HealthStatus.Unhealthy.ToString(), response.Status);
    }
}
