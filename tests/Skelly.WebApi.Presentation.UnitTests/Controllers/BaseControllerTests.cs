using Microsoft.AspNetCore.Mvc.Infrastructure;
using Skelly.WebApi.Presentation.Controllers;

namespace Skelly.WebApi.Presentation.UnitTests.Controllers;

public class BaseControllerTests
{
    private readonly Mock<ProblemDetailsFactory> _factory = new();
    private readonly SampleController _controller;

    public BaseControllerTests()
    {
        _controller = new SampleController(_factory.Object);

        _factory
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
    }

    [Fact]
    public void GivenOkResult_WhenConvertingToActionResult_ThenReturnsOk()
    {
        // Given
        var result = Result.Ok(new SampleDto());

        // When
        var actionResult = _controller.TestToActionResult(result);

        // Then
        var objectResult = Assert.IsType<OkObjectResult>(actionResult);
        var response = Assert.IsType<SampleDto>(objectResult.Value);
        Assert.Equal(result.Value, response);
    }

    [Fact]
    public void GivenNoContentResult_WhenConvertingToActionResult_ThenReturnsNoContent()
    {
        // Given
        var result = Result.NoContent();

        // When
        var actionResult = _controller.TestToActionResult(result);

        // Then
        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public void GivenBadRequestResult_WhenConvertingToActionResult_ThenReturnsBadRequest()
    {
        // Given
        var errorItem = new ErrorItemFaker().Generate();
        var result = Result.BadRequest(errorItem);

        // When
        var actionResult = _controller.TestToActionResult(result);

        // Then
        var objectResult = Assert.IsType<BadRequestObjectResult>(actionResult);
        var response = Assert.IsType<ProblemDetails>(objectResult.Value);

        Assert.Equal(StatusCodes.Status400BadRequest, response.Status);
        Assert.Equal("Bad Request", response.Title);
        Assert.Equal("One or more validation errors occurred.", response.Detail);

        var errors = Assert.IsType<ErrorItem[]>(response.Extensions["errors"]);
        Assert.Single(errors);
    }

    [Fact]
    public void GivenNotFoundResult_WhenConvertingToActionResult_ThenReturnsNotFound()
    {
        // Given
        var result = Result.NotFound();

        // When
        var actionResult = _controller.TestToActionResult(result);

        // Then
        Assert.IsType<NotFoundResult>(actionResult);
    }

    [Fact]
    public void GivenUnknownResultStatus_WhenConvertingToActionResult_ThenThrowsNotImplementedException()
    {
        // Given
        var result = new Result((ResultStatus)999);

        // When & Then
        Assert.Throws<NotImplementedException>(() => _controller.TestToActionResult(result));
    }

    private record SampleDto;

    private class SampleController(ProblemDetailsFactory factory) : BaseController(factory)
    {
        public IActionResult TestToActionResult(Result<SampleDto> result)
        {
            return ToActionResult(result);
        }
    }
}
