namespace Core.Http.Features.TokenManagers.Models;

public abstract class BaseAccessTokenGenerateRequest
{
    public string TokenEndpointUrl { get; set; } = string.Empty;
    public abstract string GetRequestKey();
}