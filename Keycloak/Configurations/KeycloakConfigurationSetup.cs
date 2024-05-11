using Ardalis.GuardClauses;
using Core.Utils;
using Keycloak.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Keycloak.Configurations;

public class KeycloakConfigurationSetup(IConfiguration configuration) : IConfigureOptions<KeycloakConfigurationModel>
{
    public void Configure(KeycloakConfigurationModel options)
    {
        var keycloakConfiguration = configuration.GetSection("KeycloakConfiguration") ??
                                    throw new ApplicationException("Конфигурация для keycloak не задана!");
        Validate(keycloakConfiguration.Get<KeycloakConfigurationModel>());
        keycloakConfiguration.Bind(options);
    }

    private void Validate(KeycloakConfigurationModel? config)
    {
        Defend.Against.Null(config, nameof(config));
        Defend.Against.NullOrEmpty(config.BaseUrl, nameof(config.BaseUrl));
        Defend.Against.NullOrEmpty(config.ClientId, nameof(config.ClientId));
        Defend.Against.NullOrEmpty(config.ClientSecret, nameof(config.ClientSecret));
        Defend.Against.NullOrEmpty(config.Audiences, nameof(config.Audiences));
        Defend.Against.NullOrEmpty(config.Realm, nameof(config.Realm));
        Defend.Against.Default(config.ExpirationTimeBySeconds, nameof(config.ExpirationTimeBySeconds));
    }

}