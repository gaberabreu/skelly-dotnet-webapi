using Skelly.WebApi.Presentation.Responses;

namespace Skelly.WebApi.Presentation.UnitTests.Responses;

public class HealthCheckDependencyTests
{
    [Fact]
    public void GivenParameters_WhenInstantiating_ThenHasCorrectValues()
    {
        // Given
        var name = new Faker().Company.CompanyName();
        var status = new Faker().PickRandom<HealthStatus>().ToString();
        var duration = new Faker().Random.Double();
        var description = new Faker().Lorem.Paragraph();
        var errorMessage = new Faker().Lorem.Sentences(1);

        // When
        var dependency = new HealthCheckDependency
        {
            Name = name,
            Status = status,
            Duration = duration,
            Description = description,
            ErrorMessage = errorMessage
        };

        // Then
        Assert.Equal(name, dependency.Name);
        Assert.Equal(status, dependency.Status);
        Assert.Equal(duration, dependency.Duration);
        Assert.Equal(description, dependency.Description);
        Assert.Equal(errorMessage, dependency.ErrorMessage);
    }
}
