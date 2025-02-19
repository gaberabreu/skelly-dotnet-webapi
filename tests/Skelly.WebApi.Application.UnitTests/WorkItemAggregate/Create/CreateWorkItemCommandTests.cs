using Skelly.WebApi.Application.WorkItemAggregate.Create;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Create;

public class CreateWorkItemCommandTests
{
    [Fact]
    public void GivenParameters_WhenInstantiating_ThenHasCorrectValues()
    {
        // Given
        var title = new Faker().Lorem.Sentences(1);

        // When
        var command = new CreateWorkItemCommand(title);

        // Then
        Assert.Equal(title, command.Title);
    }
}
