using Bogus;
using Grpc.Core;
using GRPC.NET7.Proto;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProtoBuf.Grpc.Client;
using Serilog;

namespace GRPC.NET7.Client;

public static class Extension
{
    public static ILoggerFactory ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("Logs\\log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        var loggerFactory = LoggerFactory.Create(logger =>
        {
            logger.SetMinimumLevel(LogLevel.Debug);
            logger.AddConsole();
        }).AddSerilog();

        return loggerFactory;
    }

    public static async Task ExecutePrograms(CallInvoker callInvoker)
    {
        while (true)
        {
            Console.WriteLine("\nEnter 1 to execute Authenticate.\n" +
                              "Enter 2 to execute Create User With Demo Data\n" +
                              "Enter 3 to execute User List Async\n" +
                              "Enter 4 to execute User GetById Async\n" +
                              "Enter 0 to break.\n");
            var value = int.Parse(Console.ReadLine() ?? string.Empty);
            try
            {
                switch (value)
                {
                    case 1:
                        await AuthService.Authenticate(callInvoker);
                        break;
                    case 2:
                        await CreateUser(callInvoker);
                        break;
                    case 3:
                        await UserListAsync(callInvoker);
                        break;
                    case 4:
                        await UserGetByIdAsync(callInvoker);
                        break;
                    default:
                        continue;
                }

                if (value == 0)
                    break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }


    #region Private Methods

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

    private static async Task CreateUser(CallInvoker callInvoker)
    {
        var userClient = callInvoker.CreateGrpcService<IProtoUserService>();
        var faker = new Faker();
        var userCreateRequest = new UserCreateRequest
        {
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName,
            Email = faker.Person.Email
        };
        var userResponse = await userClient.Create(userCreateRequest);
        Console.WriteLine($"An User is Created. UserId: {JsonConvert.SerializeObject(userResponse)}");
    }

    private static async Task UserGetByIdAsync(CallInvoker callInvoker)
    {
        Console.WriteLine("Please Enter an UserId : ");
        var userId = Console.ReadLine() ?? string.Empty;
        var userClient = callInvoker.CreateGrpcService<IProtoUserService>();
        var userResponse = await userClient.GetByIdAsync(userId);
        Console.WriteLine($"Received UserResponse - {JsonConvert.SerializeObject(userResponse)}");
    }

    private static async Task UserListAsync(CallInvoker callInvoker)
    {
        var userClient = callInvoker.CreateGrpcService<IProtoUserService>();
        var userResponse = await userClient.GetAsync();
        Console.WriteLine($"Received UserResponse - {JsonConvert.SerializeObject(userResponse)}");
    }

    #endregion
}