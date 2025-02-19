using Skelly.WebApi.Application.WorkItemAggregate;

namespace Skelly.WebApi.FunctionalTests.Features.WorkItem;

public class CreateWorkItemTests : IClassFixture<PresentationFactory>
{
    private readonly HttpClient _client;

    public CreateWorkItemTests(PresentationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticTokenService.GenerateToken());
    }

    [Fact]
    public async Task GivenNewWorkItem_WhenCreating_ThenCanBeRetrieved()
    {
        // Given
        var createRequest = new CreateWorkItemRequestFaker().Generate();

        // When
        var createResponse = await _client.PostAsJsonAsync("/api/v1/work-items", createRequest);
        var createResult = await createResponse.Content.ReadFromJsonAsync<WorkItemDto>();
        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);
        Assert.NotNull(createResult);
        Assert.NotEqual(Guid.Empty, createResult.Id);
        Assert.Equal(createRequest.Title, createResult.Title);

        // Then
        var getResponse = await _client.GetAsync($"/api/v1/work-items/{createResult.Id}");
        var getResult = await getResponse.Content.ReadFromJsonAsync<WorkItemDto>();
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(getResult);
        Assert.Equal(createResult.Id, getResult.Id);
        Assert.Equal(createResult.Title, getResult.Title);
    }

    [Fact]
    public async Task GivenWorkItemWithEmptyTitle_WhenCreating_ThenReturnsBadRequest()
    {
        // Given
        var request = new CreateWorkItemRequestFaker().RuleFor(e => e.Title, _ => string.Empty).Generate();

        // When
        var response = await _client.PostAsJsonAsync("/api/v1/work-items", request);

        // Then
        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var teste = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);

        result.ShouldHaveValidationErrorFor("Title")
            .WithCode("NotEmptyValidator")
            .WithMessage("'Title' must not be empty.");
    }

    [Fact]
    public async Task GivenNoAuthentication_WhenCreating_ThenReturnsUnauthorized()
    {
        // Given
        _client.DefaultRequestHeaders.Authorization = null;

        var request = new CreateWorkItemRequestFaker().Generate();

        // When
        var response = await _client.PostAsJsonAsync("/api/v1/work-items", request);

        // Then
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
