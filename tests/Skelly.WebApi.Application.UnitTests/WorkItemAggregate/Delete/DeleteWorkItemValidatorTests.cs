using Skelly.WebApi.Application.WorkItemAggregate.Delete;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Delete;

public class DeleteWorkItemValidatorTests
{
    private readonly DeleteWorkItemValidator _validator = new();

    [Fact]
    public void GivenValidCommand_WhenValidating_ThenPasses()
    {
        // Given
        var command = new DeleteWorkItemCommandFaker().Generate();

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void GivenEmptyId_WhenValidating_ThenFails()
    {
        // Given
        var command = new DeleteWorkItemCommandFaker().RuleFor(e => e.Id, _ => Guid.Empty).Generate();

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorCode("NotEmptyValidator");
    }
}
