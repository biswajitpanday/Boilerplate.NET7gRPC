
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GRPC.NET7.Api.Protos;
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

//var userClient = new User.UserClient(channel);
//var userResponse = await userClient.GetAsync(new Empty());
//Console.WriteLine($"Received UserResponse - \nIsSuccess: {userResponse.IsSuccess}\nMessage: {userResponse.Message}\nData: {userResponse.Data}");


Console.ReadKey();
await channel.ShutdownAsync();