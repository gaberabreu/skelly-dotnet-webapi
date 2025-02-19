using Ossum.CQRS;
using Ossum.Results;
using Skelly.WebApi.Application.Common;

namespace Skelly.WebApi.Application.WorkItemAggregate.List;

public record ListWorkItemsQuery(int PageNumber, int PageSize) : IQuery<Result<PagedList<WorkItemDto>>>;
