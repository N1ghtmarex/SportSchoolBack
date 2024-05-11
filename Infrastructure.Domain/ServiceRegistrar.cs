using Abstractions.Services;
using Infrastructure.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Domain;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterDomainInfrastructureServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();

        services.AddTransient<IClientService, ClientService>();

        return services;
    }
}