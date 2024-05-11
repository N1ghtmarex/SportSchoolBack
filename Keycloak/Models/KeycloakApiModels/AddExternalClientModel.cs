using System.Text.Json.Serialization;
using Core.Http.Features.HttpClients;

namespace Keycloak.Models.KeycloakApiModels;

public class AddExternalClientModel : IHttpRequest
{
    /// <summary>
    /// Уникальный внутренний идентификатор клиента, который будет задан в Keycloak
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Уникальный отображаемый идентификатор клиента, который будет задан в Keycloak
    /// </summary>
    [JsonPropertyName("clientId")]
    public Guid ClientId { get; set; }

    /// <summary>
    /// Доступен ли ClientCredential Flow
    /// </summary>
    [JsonPropertyName("serviceAccountsEnabled")]
    public bool ServiceAccountsEnabled { get; set; } = true;

    /// <summary>
    /// Optional Scopes
    /// </summary>
    [JsonPropertyName("optionalClientScopes")]
    public List<string> OptionalClientScopes { get; set; } = new();

    /// <summary>
    /// CORS
    /// </summary>
    [JsonPropertyName("webOrigins")]
    public List<string> WebOrigins { get; set; } = ["*"];
}