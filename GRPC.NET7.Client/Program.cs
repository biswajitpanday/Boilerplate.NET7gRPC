using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using GRPC.NET7.Client;
using GRPC.NET7.Client.Helpers;

Console.WriteLine("Welcome to GRPC.NET7 Client Application.");

const string serverAddress = "https://localhost:7166";
const string userServerAddress = "https://localhost:7281";

var loggerFactory = Extension.ConfigureLogger();
//var (invoker, channel) = Extension.ConfigureChannel(serverAddress, loggerFactory);

using var channel = GrpcChannel.ForAddress(userServerAddress, new GrpcChannelOptions
{
    LoggerFactory = loggerFactory
});
var invoker = channel.Intercept(new TracerInterceptor(loggerFactory));

await Extension.ExecutePrograms(invoker);

Console.ReadKey();
await channel.ShutdownAsync();
