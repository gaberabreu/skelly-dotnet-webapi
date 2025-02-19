using FluentValidation;

namespace Skelly.WebApi.Application.WorkItemAggregate.Delete;

public class DeleteWorkItemValidator : AbstractValidator<DeleteWorkItemCommand>
{
    public DeleteWorkItemValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty();
    }
}
