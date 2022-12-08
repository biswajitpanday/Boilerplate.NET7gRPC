namespace GRPC.NET7.Api.Helpers
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
            
        }
    }
}
