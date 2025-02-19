using FluentValidation;

namespace Skelly.WebApi.Application.WorkItemAggregate.Create;

public class CreateWorkItemValidator : AbstractValidator<CreateWorkItemCommand>
{
    public CreateWorkItemValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty();
    }
}