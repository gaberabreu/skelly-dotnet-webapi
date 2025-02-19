using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Skelly.WebApi.Presentation.Configurations.Swagger.Options;

public class ConfigureDescriptionOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            var info = new OpenApiInfo()
            {
                Title = assembly.GetCustomAttribute<AssemblyTitleAttribute>()!.Title,
                Version = description.ApiVersion.ToString(),
                Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()!.Description,
            };

            options.SwaggerDoc(description.GroupName, info);
        }
    }
}
