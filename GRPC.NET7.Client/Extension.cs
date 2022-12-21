using Grpc.Net.Client;
using GRPC.NET7.Proto;
using Newtonsoft.Json;
using ProtoBuf.Grpc.Client;

namespace GRPC.NET7.Client;

public static class Extension
{
    public static async Task ExecutePrograms(GrpcChannel channel)
    {

        while (true)
        {
            Console.WriteLine("Enter 1 to execute Authenticate.\n" +
                              "Enter 2 to execute UserGetAsync\n" +
                              "Enter 3 to execute UserGetByIdAsync\n" +
                              "Enter 0 to break.");
            var value = int.Parse(Console.ReadLine() ?? string.Empty);
            try
            {
                switch (value)
                {
                    case 1:
                        await Authenticate(channel);
                        break;
                    case 2:
                        await UserGetByIdAsync(channel);
                        break;
                }

                if (value == 0)
                    break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }

    public static async Task Authenticate(GrpcChannel grpcChannel)
    {
        var authenticationClient = grpcChannel.CreateGrpcService<IAuthenticationService>();
        var authenticationResponse = await authenticationClient.Authenticate(new AuthenticationRequest
        {
            UserName = "admin",
            Password = "admin"
        });

        Console.WriteLine($"Received Authentication Response - \nToken: {authenticationResponse.AccessToken}\nExpires In: {authenticationResponse.ExpiresIn}");
    }

    public static async Task UserGetByIdAsync(GrpcChannel grpcChannel)
    {
        var userClient = grpcChannel.CreateGrpcService<IProtoUserService>();
        var userResponse = await userClient.GetAsync("ea18f616-30a3-40d2-004d-08dad5931006");
        Console.WriteLine($"Received UserResponse - {JsonConvert.SerializeObject(userResponse)}");
    }

}