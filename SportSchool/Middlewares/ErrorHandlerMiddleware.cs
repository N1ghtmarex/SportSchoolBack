using System.Net;
using System.Text.Json;
using Core.Exceptions;
using Core.Http.Features.HttpClients;
using FluentValidation;

namespace SportSchool.Middlewares;

public class ErrorHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<ErrorHandlerMiddleware>();

    public async Task Invoke(HttpContext context, IHostEnvironment environment)
    {
        try
        {
            await next(context);
        }

        catch (UnauthorizedAccessException e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            var result = JsonSerializer.Serialize(new BadResponseModel { Message = e.Message });
            _logger.LogError(e, e.Message);
            await response.WriteAsync(result);
        }
        catch (ObjectExistsException e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            var result = JsonSerializer.Serialize(new BadResponseModel { Message = e.Message });
            _logger.LogError(e, e.Message);
            await response.WriteAsync(result);
        }
        catch (BusinessLogicException e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            var result = JsonSerializer.Serialize(new BadResponseModel { Message = e.Message });
            _logger.LogError(e, e.Message);
            await response.WriteAsync(result);
        }
        catch (ApplicationException e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            var result = JsonSerializer.Serialize(new BadResponseModel { Message = e.Message });
            _logger.LogError(e, e.Message);
            await response.WriteAsync(result);
        }
        catch (ObjectNotFoundException e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.NotFound;
            var result = JsonSerializer.Serialize(new BadResponseModel { Message = e.Message });
            _logger.LogError(e, e.Message);
            await response.WriteAsync(result);
        }
        catch (ValidationException e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            var result = JsonSerializer.Serialize(new BadResponseModel { Message = e.Message });
            _logger.LogError(e, e.Message);
            await response.WriteAsync(result);
        }
        catch (ForbiddenException e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.Forbidden;
            var result = JsonSerializer.Serialize(new BadResponseModel { Message = e.Message });
            _logger.LogError(e, e.Message);
            await response.WriteAsync(result);
        }
        catch (Exception e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _logger.LogError(e, "Catch exception on middleware");
            var error = environment.IsDevelopment() ? e.ToString() : "Error catches in log-file.";
            var result = JsonSerializer.Serialize(new BadResponseModel { Message = error });
            await response.WriteAsync(result);
        }
    }
}