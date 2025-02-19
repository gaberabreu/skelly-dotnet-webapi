using Skelly.Net.Api.Web.Configurations;

namespace Skelly.WebApi.Presentation.UnitTests.Configurations.Conventions;

public class KebabCaseTransformerTests
{
    private readonly KebabCaseTransformer _transformer = new();

    [Theory]
    [InlineData("TestString", "test-string")]
    [InlineData("AnotherExample", "another-example")]
    [InlineData("SimpleTest", "simple-test")]
    [InlineData("NoChange", "no-change")]
    [InlineData("alllowercase", "alllowercase")]
    [InlineData("UPPERCASE", "uppercase")]
    [InlineData("", null)]
    [InlineData(null, null)]
    public void GivenStringInput_WhenTransforming_ThenInputIsConvertedToKebabCase(string? input, string? expected)
    {
        // When
        var result = _transformer.TransformOutbound(input);

        // Then
        Assert.Equal(expected, result);
    }
}

