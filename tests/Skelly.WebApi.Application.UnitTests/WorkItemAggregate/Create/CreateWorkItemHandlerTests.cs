using Skelly.WebApi.Application.WorkItemAggregate.Create;
using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Create;

public class CreateWorkItemHandlerTests
{
    private readonly Mock<IWorkItemRepository> _repository = new();
    private readonly CreateWorkItemHandler _handler;

    public CreateWorkItemHandlerTests()
    {
        _handler = new(_repository.Object);
    }

    [Fact]
    public async Task GivenCommand_WhenHandling_ThenCreatesWorkItem()
    {
        // Given
        var command = new CreateWorkItemCommandFaker().Generate();
        var cancellationToken = CancellationToken.None;

        WorkItem? capturedWorkItem = null;
        _repository
            .Setup(e => e.AddAsync(It.IsAny<WorkItem>(), It.IsAny<CancellationToken>()))
            .Callback<WorkItem, CancellationToken>((workItem, _) => capturedWorkItem = workItem)
            .ReturnsAsync((WorkItem w, CancellationToken _) => w);

        // When
        var result = await _handler.Handle(command, cancellationToken);

        // Then
        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.NotNull(result.Value);
        Assert.Equal(command.Title, result.Value.Title);
        Assert.NotEqual(Guid.Empty, result.Value.Id);

        Assert.NotNull(capturedWorkItem);
        Assert.Equal(command.Title, capturedWorkItem.Title);

        _repository.Verify(repo => repo.AddAsync(It.IsAny<WorkItem>(), cancellationToken), Times.Once);
    }
}
