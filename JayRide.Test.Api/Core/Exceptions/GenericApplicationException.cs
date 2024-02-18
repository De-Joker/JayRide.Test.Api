namespace JayRide.Test.Api.Core.Exceptions
{
    public class GenericApplicationException : BaseException
    {
        public GenericApplicationException(string message) : base(message)
        {
        }
        public GenericApplicationException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
