using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SportSchool.StartupConfigurations.Swagger;

public class CustomSwaggerOperationAttribute : IOperationFilter
{
    private static readonly Dictionary<string, string> _defaultResponseCode = new()
    {
        { "200", "Успешно" },
        { "400", "Ошибка предусмотренная бизнес-логикой" },
        { "401", "Пользователь неавторизован" },
        { "404", "Объект для взаимодействия не найден!" },
        { "403", "Недостаточно прав для выполнения метода" },
        { "500", "Непредусмотренная ошибка сервера" }
    };


    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Get Authorize attribute
        if (context.MethodInfo.DeclaringType != null)
        {
            var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            var authorizeAttributes = attributes.ToList();
            if (authorizeAttributes.Any())
            {
                var attr = authorizeAttributes.ToList()[0];

                // Add what should be show inside the security section
                IList<string> securityInfos = new List<string>();
                securityInfos.Add($"{nameof(AuthorizeAttribute.Policy)}:{attr.Policy}");
                securityInfos.Add($"{nameof(AuthorizeAttribute.Roles)}:{attr.Roles}");
                securityInfos.Add($"{nameof(AuthorizeAttribute.AuthenticationSchemes)}:{attr.AuthenticationSchemes}");
                foreach (var apiResponse in _defaultResponseCode)
                {
                    if (operation.Responses.TryGetValue(apiResponse.Key, out var response))
                    {
                        response.Description = apiResponse.Value;
                    }
                }

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new()
                    {
                        [
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = KeycloakAuthConfiguration.AdminApiScheme
                                }
                            }
                        ] = new[] { KeycloakAuthConfiguration.AdminApiScheme }
                    }
                };
            }
            else
            {
                operation.Security.Clear();
            }
        }
    }

    private static Dictionary<string, OpenApiMediaType> GetDefaultContent()
    {
        return new Dictionary<string, OpenApiMediaType> { ["application/json"] = new() };
    }
}