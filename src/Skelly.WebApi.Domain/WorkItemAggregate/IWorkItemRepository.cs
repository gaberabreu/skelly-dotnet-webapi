namespace Skelly.WebApi.Domain.WorkItemAggregate;

public interface IWorkItemRepository
{
    Task<IEnumerable<WorkItem>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<WorkItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<WorkItem> AddAsync(WorkItem workItem, CancellationToken cancellationToken = default);

    Task<WorkItem> UpdateAsync(WorkItem workItem, CancellationToken cancellationToken = default);

    Task DeleteAsync(WorkItem workItem, CancellationToken cancellationToken = default);
}
