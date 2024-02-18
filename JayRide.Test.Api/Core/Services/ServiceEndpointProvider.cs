using JayRide.Test.Api.Core.Config;

namespace JayRide.Test.Api.Core.Services
{
    public class ServiceEndpointProvider
    {
        public ICollection<ServiceEndpointConfig> Endpoints { get; init; } = new List<ServiceEndpointConfig>();

        public ServiceEndpointConfig Get(string name)
        {
            return Endpoints.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
