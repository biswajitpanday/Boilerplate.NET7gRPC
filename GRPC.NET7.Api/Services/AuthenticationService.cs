using Microsoft.Extensions.Options;

namespace GRPC.NET7.Api.Services;

public class AuthenticationService : Authentication.AuthenticationBase
{
    private readonly IOptions<AppSettings> _appSettings;

    public AuthenticationService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings;
    }
    public override Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, ServerCallContext context)
    {
        var authenticationResponse = JwtAuthenticationManager.Authenticate(_appSettings, request);
        if (authenticationResponse == null)
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid User Credentials"));
        return Task.FromResult(authenticationResponse);
    }
}