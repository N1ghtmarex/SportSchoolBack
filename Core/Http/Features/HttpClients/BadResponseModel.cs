namespace Core.Http.Features.HttpClients;

public class BadResponseModel
{
    /// <summary>
    ///     Информация об ошибке
    /// </summary>
    public string Message { get; set; } = string.Empty;
}