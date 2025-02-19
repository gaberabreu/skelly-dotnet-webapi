using Ossum.CQRS;
using Ossum.Results;

namespace Skelly.WebApi.Application.WorkItemAggregate.Create;

public record CreateWorkItemCommand(string Title) : ICommand<Result<WorkItemDto>>;
