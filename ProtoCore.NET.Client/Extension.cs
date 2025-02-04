using Bogus;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProtoBuf.Grpc.Client;
using ProtoCore.NET.Client.Helpers;
using ProtoCore.NET.Proto;
using Serilog;

namespace ProtoCore.NET.Client;

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
                              "Enter 0 to exit.\n");

            var input = Console.ReadLine();
            input = input?.Trim(' ', '"', '\'');

            if (!int.TryParse(input, out var value) || value is not (0 or 1 or 2 or 3 or 4))
            {
                ConsoleExtensions.Error("Invalid input. Please enter a valid option (0, 1, 2, 3, or 4).");
                continue;
            }

            if (value == 0)
                break;

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
                }
            }
            catch (Exception e)
            {
                ConsoleExtensions.Error($"Error: {e.Message}");
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
        ConsoleExtensions.Success($"Received Authentication Response - \nToken: {authenticationResponse.AccessToken}\nExpires In: {authenticationResponse.ExpiresIn}");
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
        ConsoleExtensions.PrintResponse(userResponse);
    }

    private static async Task UserGetByIdAsync(CallInvoker callInvoker)
    {
        Console.Write("Please Enter an UserId : ");
        var userId = Console.ReadLine() ?? string.Empty;
        userId = userId.Trim(' ', '"', '\'');
        var userClient = callInvoker.CreateGrpcService<IProtoUserService>();
        var userResponse = await userClient.GetByIdAsync(userId);
        ConsoleExtensions.PrintResponse(userResponse);
    }

    private static async Task UserListAsync(CallInvoker callInvoker)
    {
        var userClient = callInvoker.CreateGrpcService<IProtoUserService>();
        var userResponse = await userClient.GetAsync();
        ConsoleExtensions.PrintResponse(userResponse);
    }

    #endregion
}