using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Skelly.WebApi.Presentation.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace Skelly.WebApi.Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class HealthCheckController(HealthCheckService service) : ControllerBase
{
    [HttpGet("liveness")]
    [SwaggerOperation(Summary = "Liveness probe", Description = "Checks if the API process is running.")]
    [SwaggerResponse(200, "API is running.", typeof(HealthCheckResponse))]
    public async Task<IActionResult> GetLiveness()
    {
        var report = await service.CheckHealthAsync(_ => false);
        var response = HealthCheckResponse.ForLiveness(report);
        return report.Status == HealthStatus.Healthy ? Ok(response) : StatusCode(503, response);

    }

    [HttpGet("readiness")]
    [SwaggerOperation(Summary = "Readiness probe", Description = "Checks if the API and its dependencies are ready.")]
    [SwaggerResponse(200, "API and dependencies are healthy.", typeof(HealthCheckResponse))]
    [SwaggerResponse(503, "One or more dependencies are unhealthy.", typeof(HealthCheckResponse))]
    public async Task<IActionResult> GetReadiness()
    {
        var report = await service.CheckHealthAsync();
        var response = HealthCheckResponse.ForReadiness(report);
        return report.Status == HealthStatus.Healthy ? Ok(response) : StatusCode(503, response);
    }
}
