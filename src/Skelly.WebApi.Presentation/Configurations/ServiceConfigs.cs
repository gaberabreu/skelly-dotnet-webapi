using Skelly.WebApi.Infrastructure;

namespace Skelly.WebApi.Presentation.Configurations;

public static class ServiceConfigs
{
    public static IServiceCollection AddServiceConfigs(this IServiceCollection services)
    {
        return services
            .AddFluentValidationConfigs()
            .AddMediatrConfigs()
            .AddInfrastructureServices();
    }
}
