using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Ossum.Results;

namespace Skelly.WebApi.Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController(ProblemDetailsFactory factory) : ControllerBase
{
    protected IActionResult ToActionResult<T>(Result<T> result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => Ok(result.Value),
            ResultStatus.NoContent => NoContent(),
            ResultStatus.BadRequest => BadRequest(CreateProblemDetails(result.Errors)),
            ResultStatus.NotFound => NotFound(),
            _ => throw new NotImplementedException(),
        };
    }

    private ProblemDetails CreateProblemDetails(IEnumerable<ErrorItem> errors)
    {
        var problemDetails = factory.CreateProblemDetails(
            HttpContext,
            statusCode: StatusCodes.Status400BadRequest,
            title: "Bad Request",
            detail: "One or more validation errors occurred."
        );

        problemDetails.Extensions["errors"] = errors;

        return problemDetails;
    }
}

