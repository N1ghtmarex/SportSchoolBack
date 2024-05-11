namespace Core.Http.Features.TokenManagers.Models;

public class TokenResponseModel
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpireTime { get; set; }
    public string? RefreshToken { get; set; }
    public string? Scopes { get; set; }
}