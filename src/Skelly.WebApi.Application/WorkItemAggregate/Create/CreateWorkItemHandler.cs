using Ossum.CQRS;
using Ossum.Results;
using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.Application.WorkItemAggregate.Create;

public class CreateWorkItemHandler(IWorkItemRepository repository) : ICommandHandler<CreateWorkItemCommand, Result<WorkItemDto>>
{
    public async Task<Result<WorkItemDto>> Handle(CreateWorkItemCommand request, CancellationToken cancellationToken)
    {
        var workItem = new WorkItem(request.Title);

        workItem = await repository.AddAsync(workItem, cancellationToken);

        return new WorkItemDto(workItem.Id, workItem.Title);
    }
}
