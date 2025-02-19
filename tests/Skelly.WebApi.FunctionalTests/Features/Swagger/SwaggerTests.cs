using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Skelly.WebApi.FunctionalTests.Features.Swagger;

public class SwaggerTests(PresentationFactory factory) : IClassFixture<PresentationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GivenApplicationIsRunning_WhenGettingSwagger_ThenReturnsOK()
    {
        // When
        var response = await _client.GetAsync("swagger");

        // Then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GivenApplicationIsRunning_WhenGettingSwaggerForEachVersion_ThenReturnsOK()
    {
        // When & Then
        var apiVersionProvider = factory.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var apiVersion in apiVersionProvider.ApiVersionDescriptions)
        {
            var response = await _client.GetAsync($"swagger/{apiVersion.GroupName}/swagger.json");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
