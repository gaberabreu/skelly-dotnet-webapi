using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Skelly.WebApi.Presentation.Responses;

public class HealthCheckResponse
{
    public required string Status { get; set; }
    public required double TotalDuration { get; set; }
    public IEnumerable<HealthCheckDependency>? Dependencies { get; set; }

    public static HealthCheckResponse ForLiveness(HealthReport report)
    {
        return new()
        {
            Status = report.Status.ToString(),
            TotalDuration = report.TotalDuration.TotalMilliseconds
        };
    }

    public static HealthCheckResponse ForReadiness(HealthReport report)
    {
        return new()
        {
            Status = report.Status.ToString(),
            TotalDuration = report.TotalDuration.TotalMilliseconds,
            Dependencies = report.Entries.Select(entry => new HealthCheckDependency()
            {
                Name = entry.Key,
                Status = entry.Value.Status.ToString(),
                Duration = entry.Value.Duration.TotalMilliseconds,
                Description = entry.Value.Description,
                ErrorMessage = entry.Value.Exception?.Message
            })
        };
    }
}
