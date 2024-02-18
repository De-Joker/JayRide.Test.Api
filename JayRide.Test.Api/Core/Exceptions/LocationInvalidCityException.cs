namespace JayRide.Test.Api.Core.Exceptions
{
    public class LocationInvalidCityException: BaseException      
    {
        public LocationInvalidCityException(string message) : base(message)
        {
        }

        public LocationInvalidCityException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
