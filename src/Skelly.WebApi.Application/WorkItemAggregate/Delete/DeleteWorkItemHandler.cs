using Ossum.CQRS;
using Ossum.Results;
using Skelly.WebApi.Domain.WorkItemAggregate;

namespace Skelly.WebApi.Application.WorkItemAggregate.Delete;

public class DeleteWorkItemHandler(IWorkItemRepository repository) : ICommandHandler<DeleteWorkItemCommand, Result>
{
    public async Task<Result> Handle(DeleteWorkItemCommand request, CancellationToken cancellationToken)
    {
        var workItem = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (workItem is null)
            return Result.NotFound();

        await repository.DeleteAsync(workItem, cancellationToken);

        return Result.NoContent();
    }
}