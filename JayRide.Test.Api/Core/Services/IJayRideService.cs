using JayRide.Test.Api.Core.Models;

namespace JayRide.Test.Api.Core.Services
{
    public interface IJayRideService
    {
        Task<LocationResponse> GetLocationAsync(string request);
        public Task<List<ListingTotalsResponse>> GetListingsAsync(int request);

        public CandidateResponse GetCandidate();
    }
}
