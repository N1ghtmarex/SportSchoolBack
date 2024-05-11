using System.Text.Json.Serialization;
using Core.Http.Features.HttpClients;

namespace Keycloak.Models.KeycloakApiModels;

public class CreateKeyckloakUserModel : IHttpRequest
{
    /// <summary>
    /// Логин пользователя (необязательный параметр)
    /// </summary>
    [JsonPropertyName("username")]
    public string? UserName { get; set; }

    /// <summary>
    /// Статус пользователя (необязательный параметр)
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Подтверждена ли почта (необязательный параметр)
    /// </summary>
    [JsonPropertyName("emailVerified")]
    public bool IsEmailVerified { get; set; }

    /// <summary>
    /// Имя (необязательный параметр)
    /// </summary>
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Фамилия (необязательный параметр)
    /// </summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    /// <summary>
    /// Почта (необязательный параметр)
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}