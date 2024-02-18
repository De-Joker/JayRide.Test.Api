namespace JayRide.Test.Api.Core.Exceptions
{
    public abstract class BaseException : Exception
    {
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError; 

        protected BaseException(string message) : base(message)
        {
        }

        protected BaseException(string message, Exception ex) : base(message, ex)
        {
        }

        protected BaseException(string message, Exception ex, Dictionary<string, object> properties) : base(message, ex)
        {
            Properties = properties;
        }
    }
}
