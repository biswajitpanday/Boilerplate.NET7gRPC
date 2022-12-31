namespace GRPC.NET7.Profile.Api.Helpers;

public class CustomException : Exception
{
    public CustomException(string message) : base(message)
    {

    }
}