using Microsoft.AspNetCore.Http;

namespace Abstractions.CommonModels;

public interface ICurrentHttpContextAccessor
{
    /// <summary>
    /// Идентификатор клиента в сервисе идентификации
    /// </summary>
    public string IdentityClientId { get; }

    /// <summary>
    /// Идентификатор пользователя в сервисе идентификации
    /// </summary>
    public string? IdentityUserId { get; }

    /// <summary>
    /// Роли пользователя
    /// </summary>
    public IEnumerable<string> UserRoles { get; }

    /// <summary>
    /// Вызываемый метод
    /// </summary>
    public string MethodName { get; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    public string? UserSurname { get; set; }

    /// <summary>
    /// Установка контекста
    /// </summary>
    /// <param name="context">контекст</param>
    void SetContext(HttpContext context);
}