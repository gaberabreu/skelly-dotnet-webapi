using Skelly.WebApi.Application.WorkItemAggregate.List;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.List;

public class ListWorkItemsQueryTests
{
    [Fact]
    public void GivenParameters_WhenInstantiating_ThenHasCorrectValues()
    {
        // Given
        var faker = new Faker();
        var pageNumber = faker.Random.Int(-100, 100);
        var pageSize = faker.Random.Int(-100, 100);

        // When
        var query = new ListWorkItemsQuery(pageNumber, pageSize);

        // Then
        Assert.Equal(pageNumber, query.PageNumber);
        Assert.Equal(pageSize, query.PageSize);
    }
}
