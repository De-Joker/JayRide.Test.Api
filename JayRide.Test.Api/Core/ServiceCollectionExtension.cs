using Microsoft.Extensions.DependencyInjection.Extensions;
using JayRide.Test.Api.Core.Config;
using JayRide.Test.Api.Core.Services;

namespace JayRide.Test.Api.Core
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceEndpointProvider = new ServiceEndpointProvider(); 
            foreach (var item in configuration.GetSection(nameof(ServiceEndpointConfig)).GetChildren())
            {
                serviceEndpointProvider.Endpoints.Add(item.Get<ServiceEndpointConfig>());
            }

            services.TryAddScoped(s => serviceEndpointProvider);

            return services;
        }

        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.TryAddTransient<IJayRideService,JayRideService>();

            return services;
        }
    }
}
