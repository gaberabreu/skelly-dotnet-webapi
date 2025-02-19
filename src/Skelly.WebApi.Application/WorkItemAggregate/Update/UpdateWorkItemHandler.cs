using Ossum.CQRS;
using Ossum.Results;
using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.Application.WorkItemAggregate.Update;

public class UpdateWorkItemHandler(IWorkItemRepository repository) : ICommandHandler<UpdateWorkItemCommand, Result<WorkItemDto>>
{
    public async Task<Result<WorkItemDto>> Handle(UpdateWorkItemCommand request, CancellationToken cancellationToken)
    {
        var workItem = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (workItem is null)
            return Result.NotFound();

        workItem.Title = request.Title;

        workItem = await repository.UpdateAsync(workItem, cancellationToken);

        return new WorkItemDto(workItem.Id, workItem.Title);
    }
}