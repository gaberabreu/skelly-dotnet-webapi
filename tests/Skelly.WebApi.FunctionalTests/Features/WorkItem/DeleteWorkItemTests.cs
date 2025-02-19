using Skelly.WebApi.Application.WorkItemAggregate;

namespace Skelly.WebApi.FunctionalTests.Features.WorkItem;

public class DeleteWorkItemTests : IClassFixture<PresentationFactory>
{
    private readonly HttpClient _client;

    public DeleteWorkItemTests(PresentationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticTokenService.GenerateToken());
    }

    [Fact]
    public async Task GivenExistentWorkItem_WhenDeleting_ThenCanNotBeRetrieved()
    {
        // Given
        var createRequest = new CreateWorkItemRequestFaker().Generate();
        var createResponse = await _client.PostAsJsonAsync("/api/v1/work-items", createRequest);
        var createResult = await createResponse.Content.ReadFromJsonAsync<WorkItemDto>();
        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);
        Assert.NotNull(createResult);
        Assert.NotEqual(Guid.Empty, createResult.Id);
        Assert.Equal(createRequest.Title, createResult.Title);

        // When 
        var deleteResponse = await _client.DeleteAsync($"/api/v1/work-items/{createResult.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Then
        var getResponse = await _client.GetAsync($"/api/v1/work-items/{createResult.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task GivenEmptyId_WhenDeleting_ThenReturnsBadRequest()
    {
        // Given
        var id = Guid.Empty;

        // When
        var response = await _client.DeleteAsync($"/api/v1/work-items/{id}");

        // Then
        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);

        result.ShouldHaveValidationErrorFor("Id")
            .WithCode("NotEmptyValidator")
            .WithMessage("'Id' must not be empty.");
    }

    [Fact]
    public async Task GivenNoAuthentication_WhenDeleting_ThenReturnsUnauthorized()
    {
        // Given
        _client.DefaultRequestHeaders.Authorization = null;

        var id = Guid.NewGuid();

        // When
        var response = await _client.DeleteAsync($"/api/v1/work-items/{id}");

        // Then
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GivenNonExistentWorkItem_WhenDeleting_ThenReturnsNotFound()
    {
        // Given
        var id = Guid.NewGuid();

        // When
        var response = await _client.DeleteAsync($"/api/v1/work-items/{id}");

        // Then
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}