using Skelly.WebApi.Presentation.Requests;

namespace Skelly.WebApi.Presentation.UnitTests.Requests;

public class ListWorkItemsRequestTests
{
    [Fact]
    public void GivenParameters_WhenInstantiating_ThenHasCorrectValues()
    {
        // Given
        var pageNumber = new Faker().Random.Int();
        var pageSize = new Faker().Random.Int();

        // When
        var request = new ListWorkItemsRequest() { PageNumber = pageNumber, PageSize = pageSize };

        // Then
        Assert.Equal(pageNumber, request.PageNumber);
        Assert.Equal(pageSize, request.PageSize);
    }
}
