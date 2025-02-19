using System.Reflection;
using FluentValidation;
using Skelly.WebApi.Application.WorkItemAggregate.Create;

namespace Skelly.WebApi.Presentation.Configurations;

public static class FluentValidationConfigs
{
    public static IServiceCollection AddFluentValidationConfigs(this IServiceCollection services)
    {
        var assemblies = new[]
        {
            Assembly.GetAssembly(typeof(CreateWorkItemValidator))
        };

        services.AddValidatorsFromAssemblies(assemblies);

        return services;
    }
}
