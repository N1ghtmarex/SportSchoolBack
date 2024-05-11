namespace Core.Http.Features.TokenManagers.Models;

public class OidcClientCredentialRequest : BaseAccessTokenGenerateRequest
{
    public string ClientId { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;

    public override string GetRequestKey()
    {
        string tokenKey = string.Join(" ", "client_credentials", ClientId, Scopes);
        return tokenKey;
    }
}