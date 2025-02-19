using Ossum.CQRS;
using Ossum.Results;

namespace Skelly.WebApi.Application.WorkItemAggregate.Delete;

public record DeleteWorkItemCommand(Guid Id) : ICommand<Result>;
