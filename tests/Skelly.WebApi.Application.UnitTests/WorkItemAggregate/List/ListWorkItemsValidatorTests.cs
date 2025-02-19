using Skelly.WebApi.Application.WorkItemAggregate.List;

namespace Skelly.WebApi.Application.UnitTests.WorkItemAggregate.List;

public class ListWorkItemsValidatorTests
{
    private readonly ListWorkItemsValidator _validator = new();

    [Fact]
    public void GivenValidQuery_WhenValidating_ThenPasses()
    {
        // Given
        var query = new ListWorkItemsQueryFaker().Generate();

        // When
        var result = _validator.TestValidate(query);

        // Then
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void GivenPageNumberLessOrEqualThanZero_WhenValidating_ThenFails()
    {
        // Given
        var query = new ListWorkItemsQueryFaker().RuleFor(e => e.PageNumber, f => f.Random.Int(-100, 0)).Generate();

        // When
        var result = _validator.TestValidate(query);

        // Then
        result.ShouldHaveValidationErrorFor(x => x.PageNumber)
            .WithErrorCode("GreaterThanValidator");
    }

    [Fact]
    public void GivenPageSizeLessOrEqualThanZero_WhenValidating_ThenFails()
    {
        // Given
        var query = new ListWorkItemsQueryFaker().RuleFor(e => e.PageSize, f => f.Random.Int(-100, 0)).Generate();

        // When
        var result = _validator.TestValidate(query);

        // Then
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorCode("GreaterThanValidator");
    }
}
