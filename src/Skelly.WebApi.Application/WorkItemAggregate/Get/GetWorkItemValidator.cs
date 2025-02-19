using FluentValidation;

namespace Skelly.WebApi.Application.WorkItemAggregate.Get;

public class GetWorkItemValidator : AbstractValidator<GetWorkItemQuery>
{
    public GetWorkItemValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty();
    }
}
