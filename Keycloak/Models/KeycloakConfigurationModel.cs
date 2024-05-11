namespace Keycloak.Models;

public class KeycloakConfigurationModel
{
    /// <summary>
    /// Identity service base url
    /// </summary>
    public required string BaseUrl { get; set; }

    /// <summary>
    /// ClientId (login)
    /// </summary>
    public required string ClientId { get; set; }

    /// <summary>
    /// ClientSecret (password)
    /// </summary>
    public required string ClientSecret { get; set; }

    /// <summary>
    /// External clients auth config
    /// </summary>
    public required ExternalClientConfigurationModel ExternalClientConfiguration { get; set; }

    /// <summary>
    /// Audiences
    /// </summary>
    public required string Audiences { get; set; }

    /// <summary>
    /// Realm
    /// </summary>
    public required string Realm { get; set; }

    /// <summary>
    /// Время испарения токена
    /// </summary>
    public int ExpirationTimeBySeconds { get; set; }
}