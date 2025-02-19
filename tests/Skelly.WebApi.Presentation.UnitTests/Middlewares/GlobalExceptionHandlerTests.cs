using Microsoft.AspNetCore.Mvc.Infrastructure;
using Skelly.WebApi.Presentation.Middlewares;

namespace Skelly.WebApi.Presentation.UnitTests.Middlewares;

public class GlobalExceptionHandlerTests
{
    private readonly Mock<ILogger<GlobalExceptionHandler>> logger = new();
    private readonly Mock<ProblemDetailsFactory> _problemDetailsFactory = new();
    private readonly DefaultHttpContext _context = new();
    private readonly GlobalExceptionHandler _handler;

    public GlobalExceptionHandlerTests()
    {
        _problemDetailsFactory
            .Setup(f => f.CreateProblemDetails(It.IsAny<HttpContext>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns((HttpContext context, int status, string title, string type, string detail, string instance) =>
                new ProblemDetails
                {
                    Status = status,
                    Title = title,
                    Type = type,
                    Detail = detail,
                    Instance = instance
                }
            );

        _context.TraceIdentifier = new Faker().Random.Guid().ToString();
        _context.Request.Path = new Faker().Internet.UrlRootedPath();
        _context.Response.Body = new MemoryStream();

        _handler = new(logger.Object, _problemDetailsFactory.Object);
    }

    [Fact]
    public async Task GivenValidationException_WhenHandling_ThenReturnsBadRequest()
    {
        // Given
        var exception = new FluentValidation.ValidationException(
            new Faker().Lorem.Word(),
            [
                new() { ErrorCode = new Faker().Random.AlphaNumeric(5), ErrorMessage = new Faker().Lorem.Sentence() }
            ]);

        // When
        var result = await _handler.TryHandleAsync(_context, exception, CancellationToken.None);

        // Then
        Assert.True(result);
        Assert.Equal((int)HttpStatusCode.BadRequest, _context.Response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", _context.Response.ContentType);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var jsonResponse = await new StreamReader(_context.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ProblemDetails>(jsonResponse);

        Assert.NotNull(errorResponse);
        Assert.Equal(StatusCodes.Status400BadRequest, errorResponse.Status);
        Assert.Equal("Bad Request", errorResponse.Title);
        Assert.Equal("One or more validation errors occurred.", errorResponse.Detail);
        Assert.NotNull(errorResponse.Extensions["errors"]);
    }

    [Fact]
    public async Task GivenException_WhenHandling_ThenReturnsInternalServerError()
    {
        // Given
        var exception = new Exception(new Faker().Lorem.Word());

        // When
        var result = await _handler.TryHandleAsync(_context, exception, CancellationToken.None);

        // Then
        Assert.True(result);
        Assert.Equal((int)HttpStatusCode.InternalServerError, _context.Response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", _context.Response.ContentType);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var jsonResponse = await new StreamReader(_context.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ProblemDetails>(jsonResponse);

        Assert.NotNull(errorResponse);
        Assert.Equal(StatusCodes.Status500InternalServerError, errorResponse.Status);
        Assert.Equal("Internal Server Error", errorResponse.Title);
        Assert.Equal("An unexpected error occurred. Please, try again later.", errorResponse.Detail);

        logger.VerifyLog(LogLevel.Error, $"An unexpected error occurred: {exception.Message}", Times.Once);
    }
}

