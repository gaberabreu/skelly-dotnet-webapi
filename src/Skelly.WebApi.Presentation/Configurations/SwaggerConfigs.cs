using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Skelly.WebApi.Infrastructure.Options;
using Skelly.WebApi.Presentation.Configurations.Swagger.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Skelly.WebApi.Presentation.Configurations;

public static class SwaggerConfigs
{
    public static IServiceCollection AddSwaggerConfigs(this IServiceCollection services)
    {
        return services
            .AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
            })
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureDescriptionOptions>()
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSecurityOptions>();
    }

    public static IApplicationBuilder UseSwaggerConfigs(this IApplicationBuilder app)
    {
        var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        var keycloak = app.ApplicationServices.GetRequiredService<IOptions<KeycloakOptions>>();

        return app
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                foreach (var groupName in provider.ApiVersionDescriptions.Select(d => d.GroupName))
                {
                    options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpper());
                }

                options.OAuthClientId(keycloak.Value.ClientId);
                options.OAuthClientSecret(keycloak.Value.ClientSecret);
                options.EnablePersistAuthorization();
            });
    }
}
