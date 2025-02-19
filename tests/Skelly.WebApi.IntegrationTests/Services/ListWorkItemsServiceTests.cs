using Skelly.WebApi.Application.WorkItemAggregate.List;
using Skelly.WebApi.Infrastructure.Persistence.Services;

namespace Skelly.WebApi.IntegrationTests.Services;

public class ListWorkItemsServiceTests(AppDbContextFixture fixture) : IClassFixture<AppDbContextFixture>
{
    private readonly AppDbContextFixture _fixture = fixture;
    private readonly ListWorkItemsService _service = new(fixture.DbContext);

    [Fact]
    public async Task GivenExistentWorkItems_WhenRetrieving_ThenReturnsExpectedAmount()
    {
        // Given
        var workItems = new WorkItemFaker().Generate(10);
        await _fixture.DbContext.WorkItems.AddRangeAsync(workItems);
        await _fixture.DbContext.SaveChangesAsync();

        // When
        var result = await _service.ListAsync(new ListWorkItemsQuery(1, 5));

        // Then
        Assert.NotNull(result);
        Assert.Equal(workItems.Count, result.TotalCount);
        Assert.Equal(5, result.Items.Count());
    }
}