using System.Reflection;
using MediatR;
using Ossum.CQRS;
using Skelly.WebApi.Application.WorkItemAggregate.Create;

namespace Skelly.WebApi.Presentation.Configurations;

public static class MediatrConfigs
{
    public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
    {
        var assemblies = new[]
        {
            Assembly.GetAssembly(typeof(CreateWorkItemCommand))
        };

        return services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(assemblies!);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            })
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }
}
