namespace JayRide.Test.Api.Core.Models
{
    public class Travel
    {
        public string From { get; init; }
        public string To { get; init; }
        public List<Listing> Listings { get; init; }
    }
}
