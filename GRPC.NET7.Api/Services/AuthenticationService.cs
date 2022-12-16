using GRPC.NET7.Proto;
using Microsoft.Extensions.Options;
using ProtoBuf.Grpc;

namespace GRPC.NET7.Api.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IOptions<AppSettings> _appSettings;

    public AuthenticationService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings;
    }

    public Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, CallContext context = default)
    {
        var authenticationResponse = JwtAuthenticationManager.Authenticate(_appSettings, request);
        if (authenticationResponse == null)
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid User Credentials"));
        return Task.FromResult(authenticationResponse);
    }
}