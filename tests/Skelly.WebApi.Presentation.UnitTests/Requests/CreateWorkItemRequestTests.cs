using Skelly.WebApi.Presentation.Requests;

namespace Skelly.WebApi.Presentation.UnitTests.Requests;

public class CreateWorkItemRequestTests
{
    [Fact]
    public void GivenParameters_WhenInstantiating_ThenHasCorrectValues()
    {
        // Given
        var title = new Faker().Lorem.Sentence(1);

        // When
        var request = new CreateWorkItemRequest() { Title = title };

        // Then
        Assert.Equal(title, request.Title);
    }
}
