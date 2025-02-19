using Skelly.WebApi.Application.WorkItemAggregate.Get;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Get;

public class GetWorkItemQueryTests
{
    [Fact]
    public void GivenParameters_WhenInstantiating_ThenHasCorrectValues()
    {
        // Given
        var id = Guid.NewGuid();

        // When
        var query = new GetWorkItemQuery(id);

        // Then
        Assert.Equal(id, query.Id);
    }
}
