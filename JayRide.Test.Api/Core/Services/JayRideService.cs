using Newtonsoft.Json.Linq;
using JayRide.Test.Api.Core.Exceptions;
using JayRide.Test.Api.Core.Models;

namespace JayRide.Test.Api.Core.Services
{
    public class JayRideService : IJayRideService
    {
        private readonly ILogger<JayRideService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServiceEndpointProvider _serviceEndpointProvider;

        public JayRideService(ILogger<JayRideService> logger, IHttpClientFactory httpClientFactory, ServiceEndpointProvider serviceEndpointProvider)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _serviceEndpointProvider = serviceEndpointProvider;
        }

        public CandidateResponse GetCandidate()
        {
            return new CandidateResponse()
            {
                Name = "test",
                Phone = "test"
            };
        }
        public async Task<LocationResponse> GetLocationAsync(string request)
        {
            var config = _serviceEndpointProvider.Get("Location");

            using var httpClient = _httpClientFactory.CreateClient(string.Empty);
            httpClient.BaseAddress = new Uri(config.Url);

            var response = await httpClient.GetAsync($"{request}/city");

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogError("JayRideService > GetLocationAsync > Error > api failed to return valid city > {@request}", request);
                throw new LocationInvalidCityException("api failed to return valid city")
                {
                    Properties = new Dictionary<string, object>
                    {
                        { "IpAddress", request}
                    }
                };
            }

            return new() { IpAddress = request, City = content };
        }

        public async Task<List<ListingTotalsResponse>> GetListingsAsync(int request)
        {
            var config = _serviceEndpointProvider.Get("Jayride");

            using var httpClient = _httpClientFactory.CreateClient(string.Empty);
            httpClient.BaseAddress = new Uri(config.Url);

            var response = await httpClient.GetAsync("QuoteRequest");

            var content = await response.Content.ReadFromJsonAsync<Travel>();

            if (content == null)
            {
                _logger.LogError("JayRideService > GetListingsAsync > Error > api failed to return listings > {@request}", request);
                throw new LocationInvalidCityException("api failed to return listings")
                {
                    Properties = new Dictionary<string, object>
                    {
                        { "NumberOfPassengers", request}
                    }
                };
            }

            var data = (from listing in content.Listings
                where listing.VehicleType.MaxPassengers >= request
                select new ListingTotalsResponse
                {
                    Listing = listing,
                    TotalPrice = listing.PricePerPassenger * request
                }).OrderBy(x=>x.TotalPrice).ToList();

            return data;
        }
    }
}