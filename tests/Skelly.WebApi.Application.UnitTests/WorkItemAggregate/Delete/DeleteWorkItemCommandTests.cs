using Skelly.WebApi.Application.WorkItemAggregate.Delete;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Delete;

public class DeleteWorkItemCommandTests
{
    [Fact]
    public void GivenParameters_WhenInstantiating_ThenHasCorrectValues()
    {
        // Given
        var id = Guid.NewGuid();

        // When
        var command = new DeleteWorkItemCommand(id);

        // Then
        Assert.Equal(id, command.Id);
    }
}
