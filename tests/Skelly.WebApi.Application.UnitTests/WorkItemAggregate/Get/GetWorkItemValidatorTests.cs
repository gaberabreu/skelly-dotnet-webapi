using Skelly.WebApi.Application.WorkItemAggregate.Get;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.Get;

public class GetWorkItemValidatorTests
{
    private readonly GetWorkItemValidator _validator = new();

    [Fact]
    public void GivenValidQuery_WhenValidating_ThenPasses()
    {
        // Given
        var query = new GetWorkItemQueryFaker().Generate();

        // When
        var result = _validator.TestValidate(query);

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void GivenEmptyId_WhenValidating_ThenFails()
    {
        // Given
        var query = new GetWorkItemQueryFaker().RuleFor(e => e.Id, _ => Guid.Empty).Generate();

        // When
        var result = _validator.TestValidate(query);

        // Then
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorCode("NotEmptyValidator");
    }
}
