namespace Skelly.WebApi.Presentation.Responses;

public class HealthCheckDependency
{
    public required string Name { get; set; }
    public required string Status { get; set; }
    public required double Duration { get; set; }
    public string? Description { get; set; }
    public string? ErrorMessage { get; set; }
}