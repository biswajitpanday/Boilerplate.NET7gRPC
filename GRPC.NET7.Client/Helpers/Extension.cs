using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GRPC.NET7.Api.Protos;
using GRPC.NET7.Client.Protos;
using Newtonsoft.Json;

namespace GRPC.NET7.Client.Helpers;

public static class Extension
{
    public static async Task ExecutePrograms(GrpcChannel channel)
    {

        while (true)
        {
            Console.WriteLine("\nEnter 1 to execute Authenticate.\n" +
                              "Enter 2 to execute UserGetAsync\n" +
                              "Enter 3 to execute UserGetByIdAsync\n" +
                              "Enter 0 to break.\n");
            var value = int.Parse(Console.ReadLine() ?? string.Empty);
            try
            {
                switch (value)
                {
                    case 1:
                        await Authenticate(channel);
                        break;
                    case 2:
                        await UserGetAsync(channel);
                        break;
                    case 3:
                        await UserGetByIdAsync(channel);
                        break;
                    default:
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

    private static async Task Authenticate(GrpcChannel grpcChannel)
    {
        var authenticationClient = new Authentication.AuthenticationClient(grpcChannel);
        var authenticationResponse = authenticationClient.Authenticate(new AuthenticationRequest
        {
            UserName = "admin",
            Password = "admin"
        });

        //var authenticationClient = grpcChannel.CreateGrpcService<IAuthenticationService>();
        //var authenticationResponse = await authenticationClient.Authenticate(new AuthenticationRequest
        //{
        //    UserName = "admin",
        //    Password = "admin"
        //});

        Console.WriteLine($"Received Authentication Response - \nToken: {authenticationResponse.AccessToken}\nExpires In: {authenticationResponse.ExpiresIn}");
    }

    private static async Task UserGetAsync(GrpcChannel grpcChannel)
    {
        var userClient = new User.UserClient(grpcChannel);
        var userResponse = await userClient.GetAsync(new Empty());
        Console.WriteLine($"Received List<UserResponse> - {JsonConvert.SerializeObject(userResponse)}");
    }

    private static async Task UserGetByIdAsync(GrpcChannel grpcChannel)
    {
        //var userClient = new User.UserClient(grpcChannel);
        //var userResponse = await userClient.GetAsync("ea18f616-30a3-40d2-004d-08dad5931006");
        //Console.WriteLine($"Received UserResponse - {JsonConvert.SerializeObject(userResponse)}");
    }

}