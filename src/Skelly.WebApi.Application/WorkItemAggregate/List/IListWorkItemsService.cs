using Skelly.WebApi.Application.Common;

namespace Skelly.WebApi.Application.WorkItemAggregate.List;

public interface IListWorkItemsService
{
    Task<PagedList<WorkItemDto>> ListAsync(ListWorkItemsQuery query, CancellationToken cancellationToken = default);
}
