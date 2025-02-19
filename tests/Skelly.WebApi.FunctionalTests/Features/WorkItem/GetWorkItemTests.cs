namespace Skelly.WebApi.FunctionalTests.Features.WorkItem;

public class GetWorkItemTests : IClassFixture<PresentationFactory>
{
    private readonly HttpClient _client;

    public GetWorkItemTests(PresentationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticTokenService.GenerateToken());
    }

    [Fact]
    public async Task GivenEmptyId_WhenRetrieving_ThenReturnsBadRequest()
    {
        // Given
        var id = Guid.Empty;

        // When
        var response = await _client.GetAsync($"/api/v1/work-items/{id}");

        // Then
        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);

        result.ShouldHaveValidationErrorFor("Id")
            .WithCode("NotEmptyValidator")
            .WithMessage("'Id' must not be empty.");
    }

    [Fact]
    public async Task GivenNoAuthentication_WhenRetrieving_ThenReturnsUnauthorized()
    {
        // Given
        _client.DefaultRequestHeaders.Authorization = null;

        var id = Guid.NewGuid();

        // When
        var response = await _client.GetAsync($"/api/v1/work-items/{id}");

        // Then
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GivenNonExistentWorkItem_WhenRetrieving_ThenReturnsNotFound()
    {
        // Given
        var id = Guid.NewGuid();

        // When
        var response = await _client.GetAsync($"/api/v1/work-items/{id}");

        // Then
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
