using Core.Http.Features.TokenManagers.Models;

namespace Prolog.Keycloak.TokenRequestStrategies;

public class OidcUserPasswordRequest : BaseAccessTokenGenerateRequest
{
    public required string Username { get; set; }
    public string? Password { get; set; }
    public required string ClientId { get; set; }
    public required string Scopes { get; set; }
    public required string ClientSecret { get; set; }
    public override string GetRequestKey()
    {
        throw new NotImplementedException();
    }
}