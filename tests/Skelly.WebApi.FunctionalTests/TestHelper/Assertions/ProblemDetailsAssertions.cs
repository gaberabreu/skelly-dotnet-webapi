using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Ossum.Results;

namespace Skelly.WebApi.FunctionalTests.TestHelper.Assertions;

[JsonSerializable(typeof(ErrorItem[]))]
public partial class ErrorItemContext : JsonSerializerContext { }

public static class ProblemDetailsAssertions
{
    private static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        TypeInfoResolver = JsonTypeInfoResolver.Combine(ErrorItemContext.Default)

    };
    public static ProblemDetailsValidator ShouldHaveValidationErrorFor(this ProblemDetails problemDetails, string field)
    {
        var errors = problemDetails.GetErrors().Where(e => e.Field == field).ToArray();
        Assert.NotEmpty(errors);
        return new ProblemDetailsValidator(errors);
    }

    public static ProblemDetailsValidator ShouldHaveAnyValidationError(this ProblemDetails problemDetails)
    {
        var errors = problemDetails.GetErrors();
        Assert.NotEmpty(errors);
        return new ProblemDetailsValidator(errors);
    }

    private static ErrorItem[] GetErrors(this ProblemDetails problemDetails)
    {
        if (problemDetails.Extensions.TryGetValue("errors", out var errorsElement) && errorsElement is JsonElement jsonElement)
        {
            var teste = jsonElement.Deserialize<ErrorItem[]>(DefaultJsonOptions);
            return teste ?? [];
        }

        return [];
    }

    public class ProblemDetailsValidator(ErrorItem[] errors)
    {
        private readonly ErrorItem[] _errors = errors;

        public ProblemDetailsValidator WithField(string expectedField)
        {
            Assert.Contains(_errors, e => e.Field == expectedField);
            return this;
        }

        public ProblemDetailsValidator WithCode(string expectedCode)
        {
            Assert.Contains(_errors, e => e.Code == expectedCode);
            return this;
        }

        public ProblemDetailsValidator WithMessage(string expectedMessage)
        {
            Assert.Contains(_errors, e => e.Message == expectedMessage);
            return this;
        }
    }
}

