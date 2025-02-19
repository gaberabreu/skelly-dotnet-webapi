using FluentValidation;

namespace Skelly.WebApi.Application.WorkItemAggregate.List;

public class ListWorkItemsValidator : AbstractValidator<ListWorkItemsQuery>
{
    public ListWorkItemsValidator()
    {
        RuleFor(e => e.PageNumber)
            .GreaterThan(0);

        RuleFor(e => e.PageSize)
            .GreaterThan(0);
    }
}