using Ardalis.GuardClauses;
using Core.Http.Features.TokenManagers;
using Core.Http.Features.TokenManagers.Models;
using Core.Utils;
using IdentityModel.Client;

namespace Prolog.Keycloak.TokenRequestStrategies;

public class OidcUserPasswordStrategy(HttpClient httpClient) : ITokenGenerateStrategy<OidcUserPasswordRequest>
{
    public async Task<TokenResponseModel> GenerateTokenAsync(OidcUserPasswordRequest requestModel, CancellationToken cancellationToken)
    {
        Defend.Against.NullOrEmpty(requestModel.ClientId, nameof(requestModel.ClientId));
        Defend.Against.NullOrEmpty(requestModel.ClientSecret, nameof(requestModel.ClientSecret));
        Defend.Against.NullOrEmpty(requestModel.Username, nameof(requestModel.Username));
        ValidateScopes(requestModel.Scopes);
        var request = new PasswordTokenRequest()
        {
            Address = requestModel.TokenEndpointUrl,
            ClientId = requestModel.ClientId,
            ClientSecret = requestModel.ClientSecret,
            Scope = requestModel.Scopes,
            GrantType = "password",
            UserName = requestModel.Username,
            Password = requestModel.Password
        };

        var tokenResponse =
            await httpClient.RequestPasswordTokenAsync(request, cancellationToken);

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