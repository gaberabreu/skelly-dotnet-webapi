using Skelly.WebApi.Application.WorkItemAggregate.Create;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Create;

public class CreateWorkItemValidatorTests
{
    private readonly CreateWorkItemValidator _validator = new();

    [Fact]
    public void GivenValidCommand_WhenValidating_ThenPasses()
    {
        // Given
        var command = new CreateWorkItemCommandFaker().Generate();

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void GivenEmptyTitle_WhenValidating_ThenFails()
    {
        // Given
        var command = new CreateWorkItemCommandFaker().RuleFor(e => e.Title, _ => string.Empty).Generate();

        // When
        var result = _validator.TestValidate(command);

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorCode("NotEmptyValidator");
    }
}

