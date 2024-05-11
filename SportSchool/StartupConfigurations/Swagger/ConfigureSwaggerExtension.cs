using System.Xml.Linq;
using System.Xml.XPath;
using Keycloak.Models;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SportSchool.StartupConfigurations.Models;

namespace SportSchool.StartupConfigurations.Swagger;

public static class ConfigureSwaggerExtension
{
    public static void ConfigureOwnSwagger(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IOptions<KeycloakConfigurationModel>>().Value;
        var scopesConfiguration = serviceProvider.GetRequiredService<IOptions<KeycloakScopeConfigurationModel>>().Value;

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("sportschool", new OpenApiInfo { Title = "SportSchool.Api", Version = "v1" });

            c.OperationFilter<CustomSwaggerOperationAttribute>();
            c.SupportNonNullableReferenceTypes();
            Directory
                .GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly)
                .ToList()
                .ForEach(xmlFile =>
                {
                    var doc = XDocument.Load(xmlFile);
                    c.IncludeXmlComments(() => new XPathDocument(doc.CreateReader()), includeControllerXmlComments: true);
                    c.SchemaFilter<SwaggerRequiredSchemaFilter>();
                    c.SchemaFilter<SwaggerRequiredAttributeSchemaFilter>();
                });

            var authUrl = new Uri(configuration.BaseUrl + $"/realms/{configuration.Realm}/protocol/openid-connect/auth");
            var tokenUrl = new Uri(configuration.BaseUrl + $"/realms/{configuration.Realm}/protocol/openid-connect/token");

            c.AddSecurityDefinition(KeycloakAuthConfiguration.AdminApiScheme,
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = authUrl,
                            TokenUrl = tokenUrl,
                            Scopes = new Dictionary<string, string>
                            {
                                { scopesConfiguration.SportSchoolScopeName, "SportSchool.Api" },
                                { "openid", "Identifier" },
                                { "roles", "Roles User" },
                                { "profile", "Profile identity user" },
                                { "email", "User email" }
                            }
                        }
                    },
                    Scheme = "Bearer"
                });
        });
    }

    public static void ConfigureSwaggerUi(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = serviceScope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/sportschool/swagger.json", "SportSchool.Api");
            c.OAuthAppName("Swagger Client");
            c.OAuthUsePkce();
        });
    }
}