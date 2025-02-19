using Ossum.CQRS;
using Ossum.Results;
using Skelly.WebApi.Application.Common;

namespace Skelly.WebApi.Application.WorkItemAggregate.List;

public class ListWorkItemsHandler(IListWorkItemsService service) : IQueryHandler<ListWorkItemsQuery, Result<PagedList<WorkItemDto>>>
{
    public async Task<Result<PagedList<WorkItemDto>>> Handle(ListWorkItemsQuery request, CancellationToken cancellationToken)
    {
        return await service.ListAsync(request, cancellationToken);
    }
}