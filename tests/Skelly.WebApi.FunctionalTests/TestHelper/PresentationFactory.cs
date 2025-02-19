using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Skelly.WebApi.Infrastructure.HealthChecks;
using Skelly.WebApi.Infrastructure.Persistence;

namespace Skelly.WebApi.FunctionalTests.TestHelper;

public class PresentationFactory : WebApplicationFactory<Program>
{
    private readonly string TestId = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            FakeInfrastructureServices(services);
            FakeHealthConfigs(services);
            FakeBearerConfigs(services);
        });
    }

    private static void FakeHealthConfigs(IServiceCollection services)
    {
        services.Configure<HealthCheckServiceOptions>(options =>
        {
            options.Registrations.Clear();
        });

        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database", failureStatus: HealthStatus.Unhealthy);
    }

    private static void FakeBearerConfigs(IServiceCollection services)
    {
        var authDescriptors = services
            .Where(s => s.ServiceType == typeof(IConfigureOptions<AuthenticationOptions>))
            .ToList();

        foreach (var descriptor in authDescriptors)
        {
            services.Remove(descriptor);
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "test-issuer",
                    ValidAudience = "test-audience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticTokenService.SecretKey))
                };
            });

        services.AddAuthorization();
    }

    private void FakeInfrastructureServices(IServiceCollection services)
    {
        var descriptorsToRemove = services
            .Where(s => s.ServiceType == typeof(AppDbContext) ||
                        s.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                        s.ServiceType.FullName!.Contains("EntityFrameworkCore"))
            .ToList();

        foreach (var descriptor in descriptorsToRemove)
        {
            services.Remove(descriptor);
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(TestId));
    }
}
