using Core.Http.Features.TokenManagers.Models;

namespace Core.Http.Features.TokenManagers;

/// <summary>
///     Менеджер/хранилище токенов
/// </summary>
public interface IAccessTokenManager
{
    /// <summary>
    ///     Получение токена доступа.
    ///     Возвращает хранимый токен доступа, если токен не истек.
    ///     По наступлении времени испарения генерируется новый токен.
    /// </summary>
    Task<TokenResponseModel> GetAccessToken<TRequest>(ITokenGenerateStrategy<TRequest> strategy,
        TRequest requestModel, CancellationToken cancellationToken = default)
        where TRequest : BaseAccessTokenGenerateRequest;
}