using Microsoft.Extensions.Options;
using SportSchool.StartupConfigurations.Models;

namespace SportSchool.StartupConfigurations.Options;

public class KeycloakScopesConfigurationSetup(IConfiguration configuration) : IConfigureOptions<KeycloakScopeConfigurationModel>
{

    public void Configure(KeycloakScopeConfigurationModel options)
    {
        var scopesConfiguration = configuration.GetSection("KeycloakScopesConfiguration");
        Validate(scopesConfiguration.Get<KeycloakScopeConfigurationModel>());
        scopesConfiguration.Bind(options);
    }

    private void Validate(KeycloakScopeConfigurationModel? config)
    {
        if (config is null)
        {
            throw new ArgumentException("Keycloak scope configuration is null!");
        };
        
        if (config.SportSchoolScopeName is null)
        {
            throw new ArgumentException("SportSchool scope is null!");
        };
    }
}