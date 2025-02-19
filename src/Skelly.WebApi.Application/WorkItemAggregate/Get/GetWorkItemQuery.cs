using Ossum.CQRS;
using Ossum.Results;

namespace Skelly.WebApi.Application.WorkItemAggregate.Get;

public record GetWorkItemQuery(Guid Id) : IQuery<Result<WorkItemDto>>;
