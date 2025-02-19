using Skelly.WebApi.Application.WorkItemAggregate.Delete;
using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Delete;

public class DeleteWorkItemHandlerTests
{
    private readonly Mock<IWorkItemRepository> _repository = new();
    private readonly DeleteWorkItemHandler _handler;

    public DeleteWorkItemHandlerTests()
    {
        _handler = new(_repository.Object);
    }

    [Fact]
    public async Task GivenExistentWorkItem_WhenHandling_ThenDeletesWorkItem()
    {
        // Given
        var command = new DeleteWorkItemCommandFaker().Generate();
        var cancellationToken = CancellationToken.None;

        var workItem = new WorkItemFaker().Generate();
        _repository.Setup(e => e.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync(workItem);

        // When
        var result = await _handler.Handle(command, cancellationToken);

        // Then
        Assert.Equal(ResultStatus.NoContent, result.Status);
        Assert.Null(result.Value);

        _repository.Verify(e => e.GetByIdAsync(command.Id, cancellationToken), Times.Once);
        _repository.Verify(e => e.DeleteAsync(workItem, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GivenNonExistentWorkItem_WhenHandling_ThenReturnsNotFound()
    {
        // Given
        var command = new DeleteWorkItemCommandFaker().Generate();
        var cancellationToken = CancellationToken.None;

        _repository.Setup(e => e.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync((WorkItem?)null);

        // When
        var result = await _handler.Handle(command, cancellationToken);

        // Then
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Null(result.Value);

        _repository.Verify(e => e.GetByIdAsync(command.Id, cancellationToken), Times.Once);
        _repository.Verify(e => e.DeleteAsync(It.IsAny<WorkItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
