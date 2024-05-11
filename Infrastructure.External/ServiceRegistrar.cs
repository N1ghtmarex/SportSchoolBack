using Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterExternalInfrastructureServices(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        return services;
    }
}