using System.Text.Json.Serialization;

namespace Keycloak.Models.KeycloakApiModels;

public class AccessSetting
{
    /// <summary>
    /// View
    /// </summary>
    [JsonPropertyName("view")]
    public bool View { get; set; }

    /// <summary>
    /// Configure
    /// </summary>
    [JsonPropertyName("configure")]
    public bool Configure { get; set; }

    /// <summary>
    /// Manage
    /// </summary>
    [JsonPropertyName("manage")]
    public bool Manage { get; set; }
}