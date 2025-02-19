using Skelly.WebApi.Application.WorkItemAggregate.Update;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Update;

public class UpdateWorkItemValidatorTests
{
    private readonly UpdateWorkItemValidator _validator = new();

    [Fact]
    public void GivenValidCommand_WhenValidating_ThenPasses()
    {
        // Given
        var command = new UpdateWorkItemCommandFaker().Generate();

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void GivenEmptyId_WhenValidating_ThenFails()
    {
        // Given
        var command = new UpdateWorkItemCommandFaker().RuleFor(e => e.Id, _ => Guid.Empty).Generate();

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public void GivenEmptyTitle_WhenValidating_ThenFails()
    {
        // Given
        var command = new UpdateWorkItemCommandFaker().RuleFor(e => e.Title, _ => string.Empty).Generate();

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorCode("NotEmptyValidator");
    }
}
