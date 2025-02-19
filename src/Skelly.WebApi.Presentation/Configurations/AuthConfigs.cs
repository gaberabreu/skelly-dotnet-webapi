using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Skelly.WebApi.Infrastructure.Options;

namespace Skelly.WebApi.Presentation.Configurations;

public static class AuthConfigs
{
    public static IServiceCollection AddAuthConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptionsMonitor<KeycloakOptions>>((options, keycloakOptionsMonitor) =>
            {
                var keycloak = keycloakOptionsMonitor.CurrentValue;
                options.Authority = keycloak.Authority;
                options.Audience = keycloak.ClientId;
            });

        services.AddAuthorization();

        return services;
    }
}
