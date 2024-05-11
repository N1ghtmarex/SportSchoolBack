using Ardalis.GuardClauses;
using Core.Exceptions;
using Core.Utils;
using Keycloak.Abstractions;
using Keycloak.Clients;
using Keycloak.Models;
using Keycloak.Models.KeycloakApiModels;
using Microsoft.Extensions.Options;

namespace Keycloak.Services;

internal class KeycloakIdentityService(KeycloakHttpClient httpClient,
    IOptions<KeycloakConfigurationModel> configuration) : IIdentityService
{
    private readonly string? _optionalScopes = configuration.Value.ExternalClientConfiguration?.OptionalScopes;

    private readonly string _realm = configuration.Value.Realm;

    public async Task<AccessKeyViewModel> CreateExternalClientAsync(CancellationToken cancellationToken = default)
    {
        // создание клиента
        var id = Guid.NewGuid();
        var createClientBody = new AddExternalClientModel { Id = id, ClientId = id,
            OptionalClientScopes = [.. _optionalScopes?.Split(" ")]
        };
        await httpClient.PostAsync(new Uri($"admin/realms/{_realm}/clients", UriKind.Relative), createClientBody, cancellationToken);

        // получение client_secret созданного клиента
        var clientSecret = await httpClient.GetAsync<KeycloakClientSecretModel>(
            new Uri($"admin/realms/{_realm}/clients/{createClientBody.ClientId}/client-secret", UriKind.Relative), cancellationToken);

        return new AccessKeyViewModel
        {
            ClientId = createClientBody.ClientId.ToString(),
            ClientSecret = clientSecret.Value
        };
    }

    public async Task<AccessKeyViewModel> ResetClientSecretForExternalClientAsync(string clientId, CancellationToken cancellationToken = default)
    {
        Defend.Against.NullOrEmpty(clientId, nameof(clientId));
        var clientSecret = await httpClient.PostWithResultAsync<KeycloakClientSecretModel>(
            new Uri($"admin/realms/{_realm}/clients/{clientId}/client-secret", UriKind.Relative),
            cancellationToken);
        return new AccessKeyViewModel { ClientId = clientId, ClientSecret = clientSecret.Value};
    }

    public async Task DisableExternalClientAsync(string clientId, CancellationToken cancellationToken = default)
    {
        Defend.Against.NullOrEmpty(clientId, nameof(clientId));
        // получение данных о клиенте
        var externalClient =
            await httpClient.GetAsync<ExternalClientModel>(new Uri($"admin/realms/{_realm}/clients/{clientId}", UriKind.Relative), cancellationToken);
        if (externalClient == null)
        {
            throw new ObjectNotFoundException($"Внешний клиент с идентификатором {clientId} не найден!");
        }

        // блокировка клиента
        externalClient.Enabled = false;
        await httpClient.PutAsync(new Uri($"admin/realms/{_realm}/clients/{clientId}", UriKind.Relative), externalClient, cancellationToken);
    }

    public async Task EnableExternalClientAsync(string clientId, CancellationToken cancellationToken = default)
    {
        Defend.Against.NullOrEmpty(clientId, nameof(clientId));
        // получение данных о клиенте
        var externalClient =
            await httpClient.GetAsync<ExternalClientModel>(new Uri($"admin/realms/{_realm}/clients/{clientId}", UriKind.Relative), cancellationToken);
        if (externalClient == null)
        {
            throw new ObjectNotFoundException($"Внешний клиент с идентификатором {clientId} не найден!");
        }

        // разблокировка клиента
        externalClient.Enabled = true;
        await httpClient.PutAsync(new Uri($"admin/realms/{_realm}/clients/{clientId}", UriKind.Relative), externalClient, cancellationToken);
    }

    public async Task<string> CreateUserAsync(CreateKeyckloakUserModel request, CancellationToken cancellationToken = default)
    {
        Defend.Against.Null(request, nameof(request));
        request.UserName = string.IsNullOrEmpty(request.UserName) ? Guid.NewGuid().ToString() : request.UserName;

        // Создание пользователя
        await httpClient.PostAsync(new Uri($"admin/realms/{_realm}/users", UriKind.Relative), request, cancellationToken);

        // получение идентификатора созданного пользователя
        var queryParameter = new Dictionary<string, string?>{ { "username", request.UserName } };
        var createdUser = (await httpClient.GetAsync<List<UserRepresentationModel>>(new Uri($"admin/realms/{_realm}/users", UriKind.Relative),
            queryParameter, cancellationToken))
            .Single();
        return createdUser.Id;
    }

    public async Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        Defend.Against.Null(userId, nameof(userId));
        await httpClient.DeleteAsync(new Uri($"admin/realms/{_realm}/users/{userId}", UriKind.Relative), cancellationToken);
    }
}