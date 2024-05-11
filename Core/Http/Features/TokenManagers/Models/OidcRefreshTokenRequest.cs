namespace Core.Http.Features.TokenManagers.Models;

public class OidcRefreshTokenRequest : BaseAccessTokenGenerateRequest
{
    public string ClientId { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

    public override string GetRequestKey()
    {
        string tokenKey = string.Join(" ", "refresh", ClientId, Scopes);
        return tokenKey;
    }
}