using System.Text.Json.Serialization;
using Core.Http.Features.HttpClients;

namespace Keycloak.Models.KeycloakApiModels;

public class ExternalClientModel : IHttpRequest
{
    /// <summary>
    /// Настройки доступа
    /// </summary>
    [JsonPropertyName("access")]
    public required AccessSetting AccessSettings { get; set; }

    /// <summary>
    /// Всегда отображать в UI
    /// </summary>
    [JsonPropertyName("alwaysDisplayInConsole")]
    public bool AlwaysDisplayInConsole { get; set; }

    /// <summary>
    /// Аттрибуты
    /// </summary>
    [JsonPropertyName("attributes")]
    public required Dictionary<string, string> Attributes { get; set; }

    /// <summary>
    /// Bearer Only
    /// </summary>
    [JsonPropertyName("bearerOnly")]
    public bool BearerOnly { get; set; }

    /// <summary>
    /// Client Authenticator Type
    /// </summary>
    [JsonPropertyName("clientAuthenticatorType")]
    public required string ClientAuthenticatorType { get; set; }

    /// <summary>
    /// Идентификатор (внешний) клиента
    /// </summary>
    [JsonPropertyName("clientId")]
    public required string ClientId { get; set; }

    /// <summary>
    /// Consent Required
    /// </summary>
    [JsonPropertyName("consentRequired")]
    public bool ConsentRequired { get; set; }

    /// <summary>
    /// Default Client Scopes
    /// </summary>
    [JsonPropertyName("defaultClientScopes")]
    public required List<string> DefaultClientScopes { get; set; }

    /// <summary>
    /// Доступность direct Access Grants flow
    /// </summary>
    [JsonPropertyName("directAccessGrantsEnabled")]
    public bool DirectAccessGrantsEnabled { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// Frontchannel Logout
    /// </summary>
    [JsonPropertyName("frontchannelLogout")]
    public bool FrontchannelLogout { get; set; }

    /// <summary>
    /// Full Scope Allowed
    /// </summary>
    [JsonPropertyName("fullScopeAllowed")]
    public bool FullScopeAllowed { get; set; }

    /// <summary>
    /// Идентификатор (внутренний) клиента
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Доступность implicit Flow
    /// </summary>
    [JsonPropertyName("implicitFlowEnabled")]
    public bool ImplicitFlowEnabled { get; set; }

    /// <summary>
    /// Node ReRegistration Timeout
    /// </summary>
    [JsonPropertyName("nodeReRegistrationTimeout")]
    public int NodeReRegistrationTimeout { get; set; }

    /// <summary>
    /// Not Before
    /// </summary>
    [JsonPropertyName("notBefore")]
    public int NotBefore { get; set; }

    /// <summary>
    /// Optional Client Scopes
    /// </summary>
    [JsonPropertyName("optionalClientScopes")]
    public required List<string> OptionalClientScopes { get; set; }

    /// <summary>
    /// Протокол
    /// </summary>
    [JsonPropertyName("protocol")]
    public required string Protocol { get; set; }

    /// <summary>
    /// Мапперы протоколов
    /// </summary>
    [JsonPropertyName("protocolMappers")]
    public required List<ProtocolMapper> ProtocolMappers { get; set; }

    /// <summary>
    /// Публичный клиент
    /// </summary>
    [JsonPropertyName("publicClient")]
    public bool PublicClient { get; set; }

    /// <summary>
    /// Redirect Uris
    /// </summary>
    [JsonPropertyName("redirectUris")]
    public required List<string> RedirectUris { get; set; }

    /// <summary>
    /// Secret
    /// </summary>
    [JsonPropertyName("secret")]
    public required string Secret { get; set; }

    /// <summary>
    /// Service Accounts Enabled
    /// </summary>
    [JsonPropertyName("serviceAccountsEnabled")]
    public bool ServiceAccountsEnabled { get; set; }

    /// <summary>
    /// Standard Flow Enabled
    /// </summary>
    [JsonPropertyName("standardFlowEnabled")]
    public bool StandardFlowEnabled { get; set; }

    /// <summary>
    /// Surrogate Auth Required
    /// </summary>
    [JsonPropertyName("surrogateAuthRequired")]
    public bool SurrogateAuthRequired { get; set; }

    /// <summary>
    /// Web Origins
    /// </summary>
    [JsonPropertyName("webOrigins")]
    public required List<string> WebOrigins { get; set; }

    /// <summary>
    /// Authentication Flow Binding Overrides
    /// </summary>
    [JsonPropertyName("authenticationFlowBindingOverrides")]
    public required Dictionary<string, string> AuthenticationFlowBindingOverrides { get; set; }
}