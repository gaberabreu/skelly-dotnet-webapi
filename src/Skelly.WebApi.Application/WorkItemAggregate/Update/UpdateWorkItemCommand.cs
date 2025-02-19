using Ossum.CQRS;
using Ossum.Results;

namespace Skelly.WebApi.Application.WorkItemAggregate.Update;

public record UpdateWorkItemCommand(Guid Id, string Title) : ICommand<Result<WorkItemDto>>;
