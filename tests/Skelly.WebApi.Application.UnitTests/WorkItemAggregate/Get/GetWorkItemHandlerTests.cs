using Skelly.WebApi.Application.WorkItemAggregate.Get;
using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Get;

public class GetWorkItemHandlerTests
{
    private readonly Mock<IWorkItemRepository> _repository = new();
    private readonly GetWorkItemHandler _handler;

    public GetWorkItemHandlerTests()
    {
        _handler = new(_repository.Object);
    }

    [Fact]
    public async Task GivenExistentWorkItem_WhenHandling_ThenReturnsWorkItem()
    {
        // Given
        var query = new GetWorkItemQueryFaker().Generate();
        var cancellationToken = CancellationToken.None;

        var workItem = new WorkItemFaker().Generate();
        _repository.Setup(e => e.GetByIdAsync(query.Id, cancellationToken))
            .ReturnsAsync(workItem);

        // When
        var result = await _handler.Handle(query, cancellationToken);

        // Then
        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.NotNull(result.Value);
        Assert.Equal(workItem.Id, result.Value.Id);
        Assert.Equal(workItem.Title, result.Value.Title);

        _repository.Verify(e => e.GetByIdAsync(query.Id, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GivenNonExistentWorkItem_WhenHandling_ThenReturnsNotFound()
    {
        // Given
        var query = new GetWorkItemQueryFaker().Generate();
        var cancellationToken = CancellationToken.None;

        _repository.Setup(e => e.GetByIdAsync(query.Id, cancellationToken))
            .ReturnsAsync((WorkItem?)null);

        // When
        var result = await _handler.Handle(query, cancellationToken);

        // Then
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Null(result.Value);

        _repository.Verify(e => e.GetByIdAsync(query.Id, cancellationToken), Times.Once);
    }
}
