using System.Text.Json.Serialization;

namespace Keycloak.Models.KeycloakApiModels;

/// <summary>
///     Модель тела ошибки
/// </summary>
public class KeycloakBadResponseModel
{
    /// <summary>
    ///     Сообщение об ошибке от сервера
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public required string Message { get; set; }
}