using Skelly.WebApi.Infrastructure.Persistence.Repositories;

namespace Skelly.WebApi.IntegrationTests.Repositories;

public class WorkItemRepositoryTests : IClassFixture<AppDbContextFixture>
{
    private readonly WorkItemRepository _repository;

    public WorkItemRepositoryTests(AppDbContextFixture fixture)
    {
        _repository = new(fixture.DbContext);
        fixture.ResetDatabase();
    }

    [Fact]
    public async Task GivenExistentWorkItems_WhenRetrieving_ThenReturnsExpectedAmount()
    {
        // Given
        await _repository.AddAsync(new WorkItemFaker().Generate());
        await _repository.AddAsync(new WorkItemFaker().Generate());

        // When
        var workItems = await _repository.GetAllAsync();

        // Then
        Assert.NotEmpty(workItems);
        Assert.Equal(2, workItems.Count());
    }

    [Fact]
    public async Task GivenNonExistentWorkItem_WhenRetrieving_ThenReturnsNull()
    {
        // Given
        var id = Guid.NewGuid();

        // When
        var workItem = await _repository.GetByIdAsync(id);

        // Then
        Assert.Null(workItem);
    }

    [Fact]
    public async Task GivenNewWorkItem_WhenAdding_ThenCanBeRetrieved()
    {
        // Given
        var workItem = new WorkItemFaker().Generate();

        // When
        await _repository.AddAsync(workItem);

        // Then
        var retrieved = await _repository.GetByIdAsync(workItem.Id);

        Assert.NotNull(retrieved);
        Assert.Equal(workItem.Id, retrieved.Id);
        Assert.Equal(workItem.Title, retrieved.Title);
    }

    [Fact]
    public async Task GivenExistentWorkItem_WhenUpdating_ThenReflectsChanges()
    {
        // Given
        var workItem = new WorkItemFaker().Generate();
        await _repository.AddAsync(workItem);

        workItem.Title = new WorkItemFaker().Generate().Title;

        // When
        await _repository.UpdateAsync(workItem);

        // Then
        var retrieved = await _repository.GetByIdAsync(workItem.Id);

        Assert.NotNull(retrieved);
        Assert.Equal(workItem.Id, retrieved.Id);
        Assert.Equal(workItem.Title, retrieved.Title);
    }

    [Fact]
    public async Task GivenExistentWorkItem_WhenDeleting_ThenCanNotBeRetrieved()
    {
        // Given
        var workItem = new WorkItemFaker().Generate();
        await _repository.AddAsync(workItem);

        // When
        await _repository.DeleteAsync(workItem);

        // Then
        var retrieved = await _repository.GetByIdAsync(workItem.Id);
        Assert.Null(retrieved);
    }
}
