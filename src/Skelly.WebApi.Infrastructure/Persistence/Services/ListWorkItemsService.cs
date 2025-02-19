using Skelly.WebApi.Application.Common;
using Skelly.WebApi.Application.WorkItemAggregate;
using Skelly.WebApi.Application.WorkItemAggregate.List;
using Skelly.WebApi.Infrastructure.Persistence.Extensions;

namespace Skelly.WebApi.Infrastructure.Persistence.Services;

public class ListWorkItemsService(AppDbContext context) : IListWorkItemsService
{
    public async Task<PagedList<WorkItemDto>> ListAsync(ListWorkItemsQuery query, CancellationToken cancellationToken = default)
    {
        return await context.WorkItems
            .Select(e => new WorkItemDto(e.Id, e.Title))
            .ToPagedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
}
