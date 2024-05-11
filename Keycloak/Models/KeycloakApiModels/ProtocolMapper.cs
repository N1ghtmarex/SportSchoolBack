using System.Text.Json.Serialization;

namespace Keycloak.Models.KeycloakApiModels;

public class ProtocolMapper
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Протокол
    /// </summary>
    [JsonPropertyName("protocol")]
    public required string Protocol { get; set; }

    /// <summary>
    /// Protocol Mapper
    /// </summary>
    [JsonPropertyName("protocolMapper")]
    public required string ProtocolMap { get; set; }

    /// <summary>
    /// Consent Required
    /// </summary>
    [JsonPropertyName("consentRequired")]
    public bool ConsentRequired { get; set; }

    /// <summary>
    /// Конфигурация
    /// </summary>
    [JsonPropertyName("config")]
    public required ProtocolMapperConfig Config { get; set; }
}

public class ProtocolMapperConfig
{
    /// <summary>
    /// user.session.note
    /// </summary>
    [JsonPropertyName("user.session.note")]
    public required string SessionNote { get; set; }

    /// <summary>
    /// introspection.token.claim
    /// </summary>
    [JsonPropertyName("introspection.token.claim")]
    public required string IntrospectionTokenClaim { get; set; }

    /// <summary>
    /// id.token.claim
    /// </summary>
    [JsonPropertyName("id.token.claim")]
    public required string IdTokenClaim { get; set; }

    /// <summary>
    /// access.token.claim
    /// </summary>
    [JsonPropertyName("access.token.claim")]
    public required string AccessTokenClaim { get; set; }

    /// <summary>
    /// claim.name
    /// </summary>
    [JsonPropertyName("claim.name")]
    public required string ClaimName { get; set; }

    /// <summary>
    /// jsonType.label
    /// </summary>
    [JsonPropertyName("jsonType.label")]
    public required string JsonTypeLabel { get; set; }
}