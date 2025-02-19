using Skelly.WebApi.Application.WorkItemAggregate.Update;
using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Update;

public class UpdateWorkItemHandlerTests
{
    private readonly Mock<IWorkItemRepository> _repository = new();
    private readonly UpdateWorkItemHandler _handler;

    public UpdateWorkItemHandlerTests()
    {
        _handler = new(_repository.Object);
    }

    [Fact]
    public async Task GivenExistentWorkItem_WhenHandling_ThenUpdatesWorkItem()
    {
        // Given
        var command = new UpdateWorkItemCommandFaker().Generate();
        var cancellationToken = CancellationToken.None;

        var workItem = new WorkItemFaker().Generate();
        _repository.Setup(e => e.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync(workItem);

        _repository
            .Setup(e => e.UpdateAsync(workItem, cancellationToken))
            .ReturnsAsync((WorkItem w, CancellationToken _) => w);

        // When
        var result = await _handler.Handle(command, cancellationToken);

        // Then
        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.NotNull(result.Value);
        Assert.Equal(workItem.Id, result.Value.Id);
        Assert.Equal(workItem.Title, result.Value.Title);

        _repository.Verify(e => e.GetByIdAsync(command.Id, cancellationToken), Times.Once);
        _repository.Verify(e => e.UpdateAsync(workItem, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GivenNonExistentWorkItem_WhenHandling_ThenReturnsNotFound()
    {
        // Given
        var command = new UpdateWorkItemCommandFaker().Generate();
        var cancellationToken = CancellationToken.None;

        _repository.Setup(e => e.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync((WorkItem?)null);

        // When
        var result = await _handler.Handle(command, cancellationToken);

        // Then
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Null(result.Value);

        _repository.Verify(e => e.GetByIdAsync(command.Id, cancellationToken), Times.Once);
        _repository.Verify(e => e.UpdateAsync(It.IsAny<WorkItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
