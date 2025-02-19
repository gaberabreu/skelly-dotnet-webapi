using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Skelly.WebApi.Application.WorkItemAggregate.List;
using Skelly.WebApi.Domain.WorkItemAggregate;
using Skelly.WebApi.Infrastructure.HealthChecks;
using Skelly.WebApi.Infrastructure.Persistence;
using Skelly.WebApi.Infrastructure.Persistence.Repositories;
using Skelly.WebApi.Infrastructure.Persistence.Services;

namespace Skelly.WebApi.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("name=ConnectionStrings:SqlServer"));

        services.AddScoped<IWorkItemRepository, WorkItemRepository>();
        services.AddScoped<IListWorkItemsService, ListWorkItemsService>();

        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database", failureStatus: HealthStatus.Unhealthy)
            .AddCheck<KeycloakHealthCheck>("Keycloak", failureStatus: HealthStatus.Unhealthy);

        return services;
    }
}
