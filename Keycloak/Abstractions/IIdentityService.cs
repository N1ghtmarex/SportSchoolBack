using Keycloak.Models;
using Keycloak.Models.KeycloakApiModels;

namespace Keycloak.Abstractions;

public interface IIdentityService
{
    /// <summary>
    /// Добавление внешнего клиента
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Возвращает ClientId и ClientSecret созданного клиента</returns>
    Task<AccessKeyViewModel> CreateExternalClientAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление ключа доступа у внешнего клиента
    /// </summary>
    /// <param name="clientId">Идентификатор клиента</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Возвращает ClientId и ClientSecret клиента</returns>
    Task<AccessKeyViewModel> ResetClientSecretForExternalClientAsync(string clientId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Архивирование внешнего клиента
    /// </summary>
    /// <param name="clientId">Идентификатор клиента</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DisableExternalClientAsync(string clientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Разархивирование внешнего клиента
    /// </summary>
    /// <param name="clientId">Идентификатор клиента</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task EnableExternalClientAsync(string clientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Создание пользователя
    /// </summary>
    /// <param name="request">Модель пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Возвращает идентификатор созданного пользователя</returns>
    Task<string> CreateUserAsync(CreateKeyckloakUserModel request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
}