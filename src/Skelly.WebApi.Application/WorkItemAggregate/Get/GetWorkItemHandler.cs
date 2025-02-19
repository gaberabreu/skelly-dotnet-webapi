using Ossum.CQRS;
using Ossum.Results;
using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.Application.WorkItemAggregate.Get;

public class GetWorkItemHandler(IWorkItemRepository repository) : IQueryHandler<GetWorkItemQuery, Result<WorkItemDto>>
{
    public async Task<Result<WorkItemDto>> Handle(GetWorkItemQuery request, CancellationToken cancellationToken)
    {
        var workItem = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (workItem is null)
            return Result.NotFound();

        return new WorkItemDto(workItem.Id, workItem.Title);
    }
}
