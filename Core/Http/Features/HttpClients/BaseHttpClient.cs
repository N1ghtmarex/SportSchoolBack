using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using Core.Utils;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace Core.Http.Features.HttpClients
{
    /// <summary>
    ///     Базовый HTTP клиент
    /// </summary>
    public abstract class BaseHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        protected string? Scopes;
        protected string ServiceName = string.Empty;

        protected BaseHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        /// <summary>
        ///     GET запрос с query-параметрами.
        ///     Для передачи массива query-параметров использовать: new StringValues(arrayOfParameters)
        /// </summary>
        /// <typeparam name="TResponse">тип ответа</typeparam>
        /// <param name="url">url</param>
        /// <param name="queryParameters">список query-параметров</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>TResponse</returns>
        public async Task<TResponse> GetAsync<TResponse>(Uri url, Dictionary<string, string?> queryParameters,
            CancellationToken cancellationToken = default)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            
            HttpResponseMessage response =
                await _httpClient.GetAsync(QueryHelpers.AddQueryString(url.OriginalString, queryParameters),
                    cancellationToken);
            return await GetResult<TResponse>(response);
        }

        /// <summary>
        ///     GET запрос
        /// </summary>
        /// <typeparam name="TResponse">тип ответа</typeparam>
        /// <param name="url">url</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>TResponse</returns>
        public async Task<TResponse> GetAsync<TResponse>(Uri url, CancellationToken cancellationToken = default)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
            return await GetResult<TResponse>(response);
        }

        /// <summary>
        ///     POST запрос с пустым ответом
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="request">модель запроса</param>
        /// <param name="cancellationToken">токен отмены</param>
        public async Task PostAsync(Uri url, IHttpRequest request, CancellationToken cancellationToken)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            StringContent requestContent = Serialize(request);
            HttpResponseMessage response = await _httpClient.PostAsync(url, requestContent, cancellationToken);
            await CheckResult(response);
        }

        /// <summary>
        ///     POST запрос с ответом
        /// </summary>
        /// <typeparam name="TResponse">тип ответа</typeparam>
        /// <param name="url">url</param>
        /// <param name="request">модель запроса</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>TResponse</returns>
        public async Task<TResponse> PostWithResultAsync<TResponse>(Uri url, IHttpRequest<TResponse> request,
            CancellationToken cancellationToken)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            StringContent requestContent = Serialize(request);
            HttpResponseMessage response = await _httpClient.PostAsync(url, requestContent, cancellationToken);
            return await GetResult<TResponse>(response);
        }

        /// <summary>
        ///     POST запрос с ответом без тела запроса
        /// </summary>
        /// <typeparam name="TResponse">тип ответа</typeparam>
        /// <param name="url">url</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>TResponse</returns>
        public async Task<TResponse> PostWithResultAsync<TResponse>(Uri url, CancellationToken cancellationToken)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            StringContent requestContent = new StringContent(string.Empty);
            HttpResponseMessage response = await _httpClient.PostAsync(url, requestContent, cancellationToken);
            return await GetResult<TResponse>(response);
        }

        /// <summary>
        ///     POST запрос из формы с данными
        /// </summary>
        /// <typeparam name="TResponse">тип ответа</typeparam>
        /// <param name="content">данные</param>
        /// <param name="url">url</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>TResponse</returns>
        public async Task<TResponse> PostFormDataWithResultAsync<TResponse>(MultipartFormDataContent content, Uri url,
            CancellationToken cancellationToken)
        {
            Defend.Against.Null(url, nameof(url));
            Defend.Against.Default(content, nameof(content));

            await SetAuthorization();
            HttpResponseMessage response = await _httpClient.PostAsync(url, content, cancellationToken);
            return await GetResult<TResponse>(response);
        }

        /// <summary>
        ///     PUT запрос с пустым ответом
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="request">тело запроса</param>
        /// <param name="cancellationToken">токен отмены</param>
        public async Task PutAsync(Uri url, IHttpRequest request, CancellationToken cancellationToken)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            StringContent requestContent = Serialize(request);
            HttpResponseMessage response = await _httpClient.PutAsync(url, requestContent, cancellationToken);
            await CheckResult(response);
        }

        /// <summary>
        ///     PUT запрос с ответом
        /// </summary>
        /// <typeparam name="TResponse">тип ответа</typeparam>
        /// <param name="url">url</param>
        /// <param name="request">тело запроса</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>TResponse</returns>
        public async Task<TResponse> PutWithResultAsync<TResponse>(Uri url, IHttpRequest<TResponse> request,
            CancellationToken cancellationToken)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            StringContent requestContent = Serialize(request);
            HttpResponseMessage response = await _httpClient.PutAsync(url, requestContent, cancellationToken);
            return await GetResult<TResponse>(response);
        }

        /// <summary>
        ///     PATCH запрос с пустым ответом
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="request">тело запроса</param>
        /// <param name="cancellationToken">токен отмены</param>
        public async Task PatchAsync(Uri url, IHttpRequest request, CancellationToken cancellationToken)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            StringContent requestContent = Serialize(request);
            HttpResponseMessage response = await _httpClient.PatchAsync(url, requestContent, cancellationToken);
            await CheckResult(response);
        }

        /// <summary>
        ///     PATCH запрос с ответом
        /// </summary>
        /// <typeparam name="TResponse">типа ответа</typeparam>
        /// <param name="url">url</param>
        /// <param name="request">тело запроса</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>TResponse</returns>
        public async Task<TResponse> PatchWithResultAsync<TResponse>(Uri url, IHttpRequest<TResponse> request,
            CancellationToken cancellationToken)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            StringContent requestContent = Serialize(request);
            HttpResponseMessage response = await _httpClient.PatchAsync(url, requestContent, cancellationToken);
            return await GetResult<TResponse>(response);
        }

        /// <summary>
        ///     DELETE запрос без тела запроса
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="cancellationToken">токен отмены</param>
        public async Task DeleteAsync(Uri url, CancellationToken cancellationToken)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            HttpResponseMessage response = await _httpClient.DeleteAsync(url, cancellationToken);
            await CheckResult(response);
        }

        /// <summary>
        ///     DELETE запрос с телом запроса
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="request">тело запроса</param>
        /// <param name="cancellationToken">токен отмены</param>
        public async Task DeleteAsync(Uri url, IHttpRequest request,
            CancellationToken cancellationToken)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            StringContent requestContent = Serialize(request);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Content = requestContent, Method = HttpMethod.Delete, RequestUri = url
            };
            HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            await CheckResult(response);
        }

        /// <summary>
        ///     DELETE запрос с query-параметрами
        ///     Для передачи массива query-параметров использовать: new StringValues(arrayOfParameters)
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="queryParameters">query-параметры</param>
        /// <param name="cancellationToken">токен отмены</param>
        public async Task DeleteAsync(Uri url, Dictionary<string, string?> queryParameters,
            CancellationToken cancellationToken)
        {
            Defend.Against.Null(url, nameof(url));

            await SetAuthorization();
            HttpResponseMessage response =
                await _httpClient.DeleteAsync(QueryHelpers.AddQueryString(url.OriginalString, queryParameters),
                    cancellationToken);
            await CheckResult(response);
        }

        /// <summary>
        ///     Сериализация запроса
        /// </summary>
        /// <typeparam name="TResponse">тип ответа</typeparam>
        /// <param name="request">запрос</param>
        /// <returns>StringContent</returns>
        protected virtual StringContent Serialize<TResponse>(IHttpRequest<TResponse> request)
        {
            Defend.Against.Default(request, nameof(request));
            return new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        }

        /// <summary>
        ///     Сериализация запроса
        /// </summary>
        /// <param name="request">запрос</param>
        /// <returns>StringContent</returns>
        protected virtual StringContent Serialize(IHttpRequest request)
        {
            Defend.Against.Null(request, nameof(request));
            return new StringContent(JsonSerializer.Serialize(request, request.GetType()), Encoding.UTF8,
                "application/json");
        }

        /// <summary>
        ///     Получение/десериализация результата
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="response">response</param>
        protected virtual async Task<TResponse> GetResult<TResponse>(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            await CheckResult(response, content);
            return JsonSerializer.Deserialize<TResponse>(content,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? throw new JsonException(
                       $"Ошибка десериализации при выполенении запроса {response.RequestMessage?.RequestUri}.");
        }

        /// <summary>
        ///     Проверка результата на ошибки десериализации
        /// </summary>
        /// <param name="response">response</param>
        protected virtual async Task CheckResult(HttpResponseMessage response, string? content = null)
        {
            if (!response.IsSuccessStatusCode)
            {
                content ??= await response.Content.ReadAsStringAsync();
                Defend.Against.NullOrEmpty(content, nameof(content));
                try
                {
                    BadResponseModel? responseModel = JsonSerializer.Deserialize<BadResponseModel>(content);
                    throw new ApplicationException(
                        $"Статус ошибки: {response.StatusCode}. Ошибка при обращении к сервису {ServiceName}: " +
                        responseModel?.Message);
                }
                catch (JsonException)
                {
                    throw new HttpRequestException(
                        $"Статус ошибки: {response.StatusCode}. Ошибка при обращении к сервису {ServiceName}: " +
                        content);
                }
            }
        }

        /// <summary>
        ///     Установка авторизации
        /// </summary>
        protected abstract Task SetAuthorization();

        /// <summary>
        ///     Установка Scopes
        /// </summary>
        /// <param name="scopes">scopes</param>
        public void SetScopes(string scopes)
        {
            Scopes = scopes;
        }
    }
}