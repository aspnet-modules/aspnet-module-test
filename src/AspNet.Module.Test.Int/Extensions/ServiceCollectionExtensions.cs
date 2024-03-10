using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNet.Module.Test.Int.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection WithoutFluentValidators(this IServiceCollection services)
    {
        var validatorServiceDescriptions = services.Where(d =>
            d.ServiceType.Module.Name.Contains("FluentValidation", StringComparison.CurrentCultureIgnoreCase)).ToList();

        foreach (var descriptor in validatorServiceDescriptions) services.Remove(descriptor);

        return services;
    }

    public static IServiceCollection WithoutHealthChecks(this IServiceCollection services)
    {
        var healthCheckServices = services.Where(d =>
            d.ImplementationType?.FullName != null && d.ServiceType == typeof(IHostedService) &&
            d.ImplementationType != null && d.ImplementationType.FullName.Contains("Health")).ToList();

        foreach (var descriptor in healthCheckServices) services.Remove(descriptor);

        return services;
    }
}