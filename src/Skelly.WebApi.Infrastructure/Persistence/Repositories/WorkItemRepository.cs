using Microsoft.EntityFrameworkCore;
using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.Infrastructure.Persistence.Repositories;

public class WorkItemRepository(AppDbContext context) : IWorkItemRepository
{
    public async Task<IEnumerable<WorkItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.WorkItems.ToListAsync(cancellationToken);
    }

    public Task<WorkItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.WorkItems.FirstOrDefaultAsync(workItem => workItem.Id == id, cancellationToken);
    }

    public async Task<WorkItem> AddAsync(WorkItem workItem, CancellationToken cancellationToken = default)
    {
        await context.WorkItems.AddAsync(workItem, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return workItem;
    }

    public async Task<WorkItem> UpdateAsync(WorkItem workItem, CancellationToken cancellationToken = default)
    {
        context.WorkItems.Update(workItem);
        await context.SaveChangesAsync(cancellationToken);
        return workItem;
    }

    public Task DeleteAsync(WorkItem workItem, CancellationToken cancellationToken = default)
    {
        context.WorkItems.Remove(workItem);
        return context.SaveChangesAsync(cancellationToken);
    }
}
