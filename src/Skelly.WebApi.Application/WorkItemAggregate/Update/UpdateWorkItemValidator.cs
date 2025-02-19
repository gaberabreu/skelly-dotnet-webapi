using FluentValidation;

namespace Skelly.WebApi.Application.WorkItemAggregate.Update;

public class UpdateWorkItemValidator : AbstractValidator<UpdateWorkItemCommand>
{
    public UpdateWorkItemValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty();

        RuleFor(e => e.Title)
            .NotEmpty();
    }
}
