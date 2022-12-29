using Grpc.Core;
using GRPC.NET7.Proto;
using ProtoBuf.Grpc.Client;

namespace GRPC.NET7.Client;

public class AuthService
{
    public static async Task Authenticate(CallInvoker invoker)
    {
        var authenticationClient = invoker.CreateGrpcService<IAuthenticationService>();
        var authenticationResponse = await authenticationClient.Authenticate(new AuthenticationRequest
        {
            UserName = "admin",
            Password = "admin"
        });
        Console.WriteLine($"Received Authentication Response - \nToken: {authenticationResponse.AccessToken}\nExpires In: {authenticationResponse.ExpiresIn}");
    }
}