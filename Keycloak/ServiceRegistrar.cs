using Core.Http.Extensions;
using Core.Http.Features.TokenManagers;
using Core.Http.Features.TokenManagers.CommonStrategies;
using Core.Http.Features.TokenManagers.Models;
using Keycloak.Abstractions;
using Keycloak.Clients;
using Keycloak.Models;
using Keycloak.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Keycloak;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterKeycloakServices(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var keycloakConfiguration = serviceProvider.GetRequiredService<IOptions<KeycloakConfigurationModel>>().Value;

        services.AddHttpClient<KeycloakHttpClient>();
        services.AddTransient<IIdentityService, KeycloakIdentityService>();
        services.RegisterJwtTokenManagerService(keycloakConfiguration.ExpirationTimeBySeconds);
        services.AddTransient<ITokenGenerateStrategy<OidcClientCredentialRequest>, OidcClientCredentialStrategy>();
        return services;
    }
}