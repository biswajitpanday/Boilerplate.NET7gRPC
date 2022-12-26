using Bogus;
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
                        await Create(channel);
                        break;
                    case 3:
                        await UserGetByIdAsync(channel);
                        break;
                    case 4:
                        await UserListAsync(channel);
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

    private static async Task Create(GrpcChannel grpcChannel)
    {
        var userClient = grpcChannel.CreateGrpcService<IProtoUserService>();
        var faker = new Faker();
        var userCreateRequest = new UserCreateRequest
        {
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName,
            Email = faker.Person.Email
        };
        var userResponse = await userClient.Create(userCreateRequest);
        Console.WriteLine($"An User is Created. UserId: {userResponse}");
    }

    public static async Task UserGetByIdAsync(GrpcChannel grpcChannel)
    {
        var userClient = grpcChannel.CreateGrpcService<IProtoUserService>();
        var userResponse = await userClient.GetByIdAsync("ea18f616-30a3-40d2-004d-08dad5931006");
        Console.WriteLine($"Received UserResponse - {JsonConvert.SerializeObject(userResponse)}");
    }

    public static async Task UserListAsync(GrpcChannel grpcChannel)
    {
        var userClient = grpcChannel.CreateGrpcService<IProtoUserService>();
        var userResponse = await userClient.GetAsync();
        Console.WriteLine($"Received UserResponse - {JsonConvert.SerializeObject(userResponse)}");
    }

}