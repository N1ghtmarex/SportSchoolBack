using Keycloak.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SportSchool.StartupConfigurations.Models;

namespace SportSchool.StartupConfigurations;

public static class KeycloakAuthConfiguration
{
    //Policy
    public const string AdminApiPolicy = "SportSchool.Api.Policy";
    
    //Schemes
    public const string AdminApiScheme = "SportSchool.Api.Scheme";

    public static void AddKeycloakConfig(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IOptions<KeycloakConfigurationModel>>().Value;
        var scopesConfiguration = serviceProvider.GetRequiredService<IOptions<KeycloakScopeConfigurationModel>>().Value;

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = configuration.BaseUrl + $"/realms/{configuration.Realm}";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters.ValidAudiences = configuration.Audiences.Split(" ");
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AdminApiPolicy, policy =>
            {
                policy.AddAuthenticationSchemes();

                var scopes = new[] { scopesConfiguration.SportSchoolScopeName };
                policy.RequireAssertion(context => CheckScopes(context, scopes));
                policy.RequireAuthenticatedUser();
            });
        });
    }

    private static bool CheckScopes(AuthorizationHandlerContext context, string[] scopes)
    {
        var claim = context.User.FindFirst("scope");
        if (claim == null) { return false; }
        return claim.Value.Split(' ').Any(scope =>
            scopes.Contains(scope, StringComparer.Ordinal)
        );
    }
}