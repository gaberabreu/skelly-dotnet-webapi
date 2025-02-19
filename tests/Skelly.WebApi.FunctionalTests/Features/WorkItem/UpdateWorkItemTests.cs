using Skelly.WebApi.Application.WorkItemAggregate;

namespace Skelly.WebApi.FunctionalTests.Features.WorkItem;

public class UpdateWorkItemTests : IClassFixture<PresentationFactory>
{
    private readonly HttpClient _client;

    public UpdateWorkItemTests(PresentationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticTokenService.GenerateToken());
    }

    [Fact]
    public async Task GivenExistentWorkItem_WhenUpdating_ThenReflectsChanges()
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
        var updateRequest = new UpdateWorkItemRequestFaker().Generate();
        var updateResponse = await _client.PutAsJsonAsync($"/api/v1/work-items/{createResult.Id}", updateRequest);
        var updateResult = await updateResponse.Content.ReadFromJsonAsync<WorkItemDto>();
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        Assert.NotNull(updateResult);
        Assert.Equal(createResult.Id, updateResult.Id);
        Assert.Equal(updateRequest.Title, updateResult.Title);

        // Then
        var getResponse = await _client.GetAsync($"/api/v1/work-items/{updateResult.Id}");
        var getResult = await getResponse.Content.ReadFromJsonAsync<WorkItemDto>();
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(getResult);
        Assert.Equal(updateResult.Id, getResult.Id);
        Assert.Equal(updateResult.Title, getResult.Title);
    }

    [Fact]
    public async Task GivenEmptyId_WhenUpdating_ThenReturnsBadRequest()
    {
        // Given
        var id = Guid.Empty;
        var request = new UpdateWorkItemRequestFaker().Generate();

        // When
        var response = await _client.PutAsJsonAsync($"/api/v1/work-items/{id}", request);

        // Then
        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);

        result.ShouldHaveValidationErrorFor("Id")
            .WithCode("NotEmptyValidator")
            .WithMessage("'Id' must not be empty.");
    }

    [Fact]
    public async Task GivenWorkItemWithEmptyTitle_WhenUpdating_ThenReturnsBadRequest()
    {
        // Given
        var id = Guid.NewGuid();
        var request = new UpdateWorkItemRequestFaker().RuleFor(e => e.Title, _ => string.Empty).Generate();

        // When
        var response = await _client.PutAsJsonAsync($"/api/v1/work-items/{id}", request);

        // Then
        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);

        result.ShouldHaveValidationErrorFor("Title")
            .WithCode("NotEmptyValidator")
            .WithMessage("'Title' must not be empty.");
    }

    [Fact]
    public async Task GivenNoAuthentication_WhenUpdating_ThenReturnsUnauthorized()
    {
        // Given
        _client.DefaultRequestHeaders.Authorization = null;

        var id = Guid.NewGuid();
        var request = new UpdateWorkItemRequestFaker().Generate();

        // When
        var response = await _client.PutAsJsonAsync($"/api/v1/work-items/{id}", request);

        // Then
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GivenNonExistentWorkItem_WhenUpdating_ThenReturnsNotFound()
    {
        // Given
        var id = Guid.NewGuid();
        var request = new UpdateWorkItemRequestFaker().Generate();

        // When
        var response = await _client.PutAsJsonAsync($"/api/v1/work-items/{id}", request);

        // Then
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}