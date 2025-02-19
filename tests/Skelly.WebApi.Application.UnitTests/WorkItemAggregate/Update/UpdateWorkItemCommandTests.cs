using Skelly.WebApi.Application.WorkItemAggregate.Update;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Update;

public class UpdateWorkItemCommandTests
{
    [Fact]
    public void GivenParameters_WhenInstantiating_ThenHasCorrectValues()
    {
        // Given
        var id = Guid.NewGuid();
        var title = new Faker().Lorem.Sentences(1);

        // When
        var command = new UpdateWorkItemCommand(id, title);

        // Then
        Assert.Equal(id, command.Id);
        Assert.Equal(title, command.Title);
    }
}
