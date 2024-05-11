using Ardalis.GuardClauses;
using Core.Http.Features.TokenManagers.Models;
using Core.Utils;

namespace Core.Http.Features.TokenManagers;

public class AccessTokenManager(long expirationShiftInSeconds) : IAccessTokenManager
{
    private readonly Dictionary<string, TokenResponseModel> _tokenDescriptions = new();

    public async Task<TokenResponseModel> GetAccessToken<TRequest>(ITokenGenerateStrategy<TRequest> strategy,
        TRequest requestModel,
        CancellationToken cancellationToken) where TRequest : BaseAccessTokenGenerateRequest
    {
        var requestKey = requestModel.GetRequestKey();
        Defend.Against.NullOrEmpty(requestKey, nameof(requestKey));
        var tokenModel = _tokenDescriptions.GetValueOrDefault(requestKey);
        if (tokenModel == null)
        {
            tokenModel = await strategy.GenerateTokenAsync(requestModel, cancellationToken);
            _tokenDescriptions.Add(requestKey, tokenModel);
            return tokenModel;
        }

        var expireTime = DateTime.UtcNow.AddSeconds(expirationShiftInSeconds);
        if (tokenModel.ExpireTime <= expireTime)
        {
            tokenModel = await strategy.GenerateTokenAsync(requestModel, cancellationToken);
            _tokenDescriptions[requestKey] = tokenModel;
            return tokenModel;
        }

        return tokenModel;
    }
}