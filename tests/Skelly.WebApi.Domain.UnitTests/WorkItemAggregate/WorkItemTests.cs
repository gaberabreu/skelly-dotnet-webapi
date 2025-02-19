using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.Domain.UnitTests.WorkItemAggregate;

public class WorkItemTests
{
    [Fact]
    public void GivenParameters_WhenInstantiating_ThenHasCorrectValues()
    {
        // Given
        var title = new Faker().Lorem.Sentences(1);

        // When
        var workItem = new WorkItem(title);

        // Then
        Assert.Equal(title, workItem.Title);
    }
}
