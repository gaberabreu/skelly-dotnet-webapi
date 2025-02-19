using System.Net.Mime;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Ossum.Results;

namespace Skelly.WebApi.Presentation.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, ProblemDetailsFactory problemDetailsFactory) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        var response = exception switch
        {
            ValidationException validationException => HandleValidationException(validationException, httpContext),
            _ => HandleException(exception, httpContext)
        };

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }

    private ProblemDetails HandleValidationException(ValidationException exception, HttpContext httpContext)
    {
        logger.LogWarning("Validation failed: {errors}", exception.Errors);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var problemDetails = problemDetailsFactory.CreateProblemDetails(
            httpContext,
            statusCode: StatusCodes.Status400BadRequest,
            title: "Bad Request",
            detail: "One or more validation errors occurred."
        );

        problemDetails.Extensions["errors"] = exception.Errors.Select(error => new ErrorItem(error.PropertyName, error.ErrorCode, error.ErrorMessage));

        return problemDetails;
    }

    private ProblemDetails HandleException(Exception exception, HttpContext httpContext)
    {
        logger.LogError(exception, "An unexpected error occurred: {exceptionMessage}", exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = problemDetailsFactory.CreateProblemDetails(
            httpContext,
            statusCode: StatusCodes.Status500InternalServerError,
            title: "Internal Server Error",
            detail: "An unexpected error occurred. Please, try again later."
        );

        return problemDetails;
    }
}
