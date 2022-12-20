using Grpc.Net.Client;
using GRPC.NET7.Client.Helpers;

Console.WriteLine("Hello, World!");

const string serverAddress = "https://localhost:7166";
using var channel = GrpcChannel.ForAddress(serverAddress);

await Extension.ExecutePrograms(channel);

Console.ReadKey();
await channel.ShutdownAsync();