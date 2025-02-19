using Skelly.WebApi.Application.WorkItemAggregate;

namespace Skelly.WebApi.FunctionalTests.Features.WorkItem;

public class ListWorkItemsTests : IClassFixture<PresentationFactory>
{
    private readonly HttpClient _client;

    public ListWorkItemsTests(PresentationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticTokenService.GenerateToken());
    }

    [Fact]
    public async Task GivenMultipleWorkItems_WhenRetrieving_ThenReturnsOk()
    {
        // Given
        await _client.PostAsJsonAsync("/api/v1/work-items", new CreateWorkItemRequestFaker().Generate());
        await _client.PostAsJsonAsync("/api/v1/work-items", new CreateWorkItemRequestFaker().Generate());
        await _client.PostAsJsonAsync("/api/v1/work-items", new CreateWorkItemRequestFaker().Generate());
        await _client.PostAsJsonAsync("/api/v1/work-items", new CreateWorkItemRequestFaker().Generate());
        await _client.PostAsJsonAsync("/api/v1/work-items", new CreateWorkItemRequestFaker().Generate());

        // When
        var response = await _client.GetAsync("/api/v1/work-items?pageNumber=1&pageSize=5");

        // Then
        var result = await response.Content.ReadFromJsonAsync<StaticPagedList<WorkItemDto>>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(5, result.Items.Count());
    }

    [Fact]
    public async Task GivenPageNumberLessOrEqualThanZero_WhenRetrieving_ThenReturnsBadRequest()
    {
        // Given
        var request = new ListWorkItemsRequestFaker().RuleFor(e => e.PageNumber, f => f.Random.Int(-100, 0)).Generate();

        // When
        var response = await _client.GetAsync($"/api/v1/work-items?pageNumber={request.PageNumber}&pageSize={request.PageSize}");

        // Then
        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);

        result.ShouldHaveValidationErrorFor("PageNumber")
            .WithCode("GreaterThanValidator")
            .WithMessage("'Page Number' must be greater than '0'.");
    }

    [Fact]
    public async Task GivenPageSizeLessOrEqualThanZero_WhenRetrieving_ThenReturnsBadRequest()
    {
        // Given
        var request = new ListWorkItemsRequestFaker().RuleFor(e => e.PageSize, f => f.Random.Int(-100, 0)).Generate();

        // When
        var response = await _client.GetAsync($"/api/v1/work-items?pageNumber={request.PageNumber}&pageSize={request.PageSize}");

        // Then
        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);

        result.ShouldHaveValidationErrorFor("PageSize")
            .WithCode("GreaterThanValidator")
            .WithMessage("'Page Size' must be greater than '0'.");
    }

    [Fact]
    public async Task GivenNoAuthentication_WhenRetrieving_ThenReturnsUnauthorized()
    {
        // Given
        _client.DefaultRequestHeaders.Authorization = null;

        var request = new ListWorkItemsRequestFaker().Generate();

        // When
        var response = await _client.GetAsync($"/api/v1/work-items?pageNumber={request.PageNumber}&pageSize={request.PageSize}");

        // Then
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}