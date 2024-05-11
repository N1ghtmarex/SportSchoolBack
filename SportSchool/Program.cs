using Abstractions.CommonModels;
using Application;
using Domain;
using Infrastructure.Domain;
using Keycloak;
using Keycloak.Configurations;
using NLog;
using NLog.Web;
using SportSchool.Http;
using SportSchool.Middlewares;
using SportSchool.StartupConfigurations;
using SportSchool.StartupConfigurations.Options;
using SportSchool.StartupConfigurations.Swagger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var logger = LogManager.Setup().LoadConfigurationFromXml("nlog.config").GetCurrentClassLogger();
logger.Info("Инициализация SportSchool...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(LogLevel.Trace);
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    builder.Services.AddControllersWithViews();

    builder.Services.AddHealthChecks();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.RegisterDataAccessServices(builder.Configuration, builder.Environment.IsDevelopment());
    builder.Services.RegisterUseCasesServices();
    builder.Services.RegisterDomainInfrastructureServices(builder.Configuration);

    builder.Services.ConfigureOptions<KeycloakConfigurationSetup>();
    builder.Services.ConfigureOptions<KeycloakScopesConfigurationSetup>();

    builder.Services.RegisterExternalInfrastructureServices();
    builder.Services.RegisterKeycloakServices();
    builder.Services.ConfigureOwnSwagger();
    builder.Services.AddKeycloakConfig();

    builder.Services.AddScoped<ICurrentHttpContextAccessor, CurrentHttpContextAccessor>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            build =>
            {
                build
                    .AllowAnyOrigin()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("*");
            });
    });

    var app = builder.Build();

    app.MapHealthChecks("/healthz");

    app.MigrateDb();

    app.UseCors("AllowAllOrigins");

    app.ConfigureSwaggerUi();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseStaticFiles();

    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseMiddleware<ContextSetterMiddleware>();
    app.UseMiddleware<NLogRequestPostedBodyMiddleware>();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "SportSchool остановлен из-за внутренней ошибки...");
    throw;
}
finally
{
    LogManager.Shutdown();
}