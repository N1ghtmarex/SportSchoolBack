using Core.Http.Features.TokenManagers;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Http.Extensions;

public static class AccessTokenManagerExtensions
{
    /// <summary>
    ///     Регистрация в сервисах менеджера jwt-токенов (singleton)
    /// </summary>
    public static IServiceCollection RegisterJwtTokenManagerService(this IServiceCollection services, long expirationShiftInSeconds)
    {
        services.AddSingleton<IAccessTokenManager, AccessTokenManager>(x => new AccessTokenManager(expirationShiftInSeconds));
        return services;
    }
}