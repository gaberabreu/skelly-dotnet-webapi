using Skelly.WebApi.Application.Common;
using Skelly.WebApi.Application.WorkItemAggregate;
using Skelly.WebApi.Application.WorkItemAggregate.List;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.List;

public class ListWorkItemsHandlerTests
{
    private readonly Mock<IListWorkItemsService> _service = new();
    private readonly ListWorkItemsHandler _handler;

    public ListWorkItemsHandlerTests()
    {
        _handler = new(_service.Object);
    }

    [Fact]
    public async Task GivenExistentWorkItems_WhenHandling_ThenReturnsPagedList()
    {
        // Given
        var query = new ListWorkItemsQueryFaker().Generate();
        var cancellationToken = CancellationToken.None;

        var workItems = new WorkItemDtoFaker().GenerateBetween(1, 10);
        var pagedList = new PagedList<WorkItemDto>(workItems, workItems.Count, query.PageNumber, query.PageSize);
        _service.Setup(e => e.ListAsync(query, cancellationToken))
            .ReturnsAsync(pagedList);

        // When
        var result = await _handler.Handle(query, cancellationToken);

        // Then
        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.NotNull(result.Value);
        Assert.Equal(pagedList, result.Value);

        _service.Verify(e => e.ListAsync(query, cancellationToken), Times.Once);
    }
}
