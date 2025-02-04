using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using ProtoCore.NET.Client.Helpers;
using ProtoCore.NET.Client;

Console.WriteLine("Welcome to ProtoCore.NET Client Application.");

const string serverAddress = "http://localhost:5227";

var loggerFactory = Extension.ConfigureLogger();
//var (invoker, channel) = Extension.ConfigureChannel(serverAddress, loggerFactory);

using var channel = GrpcChannel.ForAddress(serverAddress, new GrpcChannelOptions
{
    LoggerFactory = loggerFactory
});
var invoker = channel.Intercept(new TracerInterceptor(loggerFactory));

await Extension.ExecutePrograms(invoker);

Console.ReadKey();
await channel.ShutdownAsync();
