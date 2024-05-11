using Ardalis.GuardClauses;
using Core.Http.Features.TokenManagers.Models;
using Core.Utils;
using IdentityModel.Client;

namespace Core.Http.Features.TokenManagers.CommonStrategies;

/// <summary>
///     Получение токена доступа через протокол OIDC. Client credentials flow
/// </summary>
public class OidcClientCredentialStrategy(HttpClient httpClient) : ITokenGenerateStrategy<OidcClientCredentialRequest>
{
    public async Task<TokenResponseModel> GenerateTokenAsync(OidcClientCredentialRequest requestModel,
        CancellationToken cancellationToken)
    {
        Defend.Against.NullOrEmpty(requestModel.ClientId, nameof(requestModel.ClientId));
        Defend.Against.NullOrEmpty(requestModel.ClientSecret, nameof(requestModel.ClientSecret));
        ValidateScopes(requestModel.Scopes);
        var request = new ClientCredentialsTokenRequest()
        {
            Address = requestModel.TokenEndpointUrl,
            ClientId = requestModel.ClientId,
            ClientSecret = requestModel.ClientSecret,
            Scope = requestModel.Scopes,
            GrantType = "client_credentials"
        };

        var tokenResponse =
            await httpClient.RequestClientCredentialsTokenAsync(request, cancellationToken);

        if (tokenResponse.IsError || string.IsNullOrEmpty(tokenResponse.AccessToken))
        {
            throw new ApplicationException("Сервер идентификации в данный момент не доступен!",
                new Exception("Ошибка авторизации! " + tokenResponse.Error));
        }

        return new TokenResponseModel
        {
            AccessToken = tokenResponse.AccessToken,
            ExpireTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
            RefreshToken = tokenResponse.RefreshToken,
            Scopes = requestModel.Scopes
        };
    }

    private void ValidateScopes(string? scopes)
    {
        if (string.IsNullOrEmpty(scopes))
        {
            return;
        }

        if (!scopes.All(c => char.IsLetterOrDigit(c) || c == '.' || c == ' '))
        {
            throw new ArgumentException("Параметр \"scopes\" имеет неверный формат! ");
        }
    }
}