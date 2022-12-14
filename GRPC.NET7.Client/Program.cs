// See https://aka.ms/new-console-template for more information

using Grpc.Net.Client;
using GRPC.NET7.Client.Protos;

Console.WriteLine("Hello, World!");

var serverAddress = "https://localhost:7166";

using var channel = GrpcChannel.ForAddress(serverAddress);

var authenticationClient = new Authentication.AuthenticationClient(channel);
var authenticationResponse = authenticationClient.Authenticate(new AuthenticationRequest
{
    UserName = "admin",
    Password = "admin"
});

Console.WriteLine($"Received Authentication Response - \nToken: {authenticationResponse.AccessToken}\nExpires In: {authenticationResponse.ExpiresIn}");

//var calculationClient = new calculatio


Console.ReadKey();
await channel.ShutdownAsync();