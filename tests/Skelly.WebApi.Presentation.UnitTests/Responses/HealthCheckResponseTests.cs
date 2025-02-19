using Skelly.WebApi.Presentation.Responses;

namespace Skelly.WebApi.Presentation.UnitTests.Responses;

public class HealthCheckResponseTests
{
    [Fact]
    public void GivenParameters_WhenInstantiating_ThenHasCorrectValues()
    {
        // Given
        var status = new Faker().PickRandom<HealthStatus>().ToString();
        var totalDuration = new Faker().Random.Double();

        // When
        var response = new HealthCheckResponse
        {
            Status = status,
            TotalDuration = totalDuration
        };

        // Then
        Assert.Equal(status, response.Status);
        Assert.Equal(totalDuration, response.TotalDuration);
    }
}
