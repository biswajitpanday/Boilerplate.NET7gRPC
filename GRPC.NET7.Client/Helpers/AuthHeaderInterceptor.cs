using Grpc.Core;
using Grpc.Core.Interceptors;
using GRPC.NET7.Proto;
using Microsoft.AspNetCore.Http;

namespace GRPC.NET7.Client.Helpers;

public class AuthHeaderInterceptor: Interceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthHeaderInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var metadata = new Metadata();
        //var authResponse = AuthService.Authenticate();
        var authResponse = new AuthenticationResponse();
        metadata.Add("authorization", $"Bearer {authResponse.AccessToken}");
        metadata.Add("expiry", $"{authResponse.ExpiresIn}");

        var userIdentity = _httpContextAccessor.HttpContext?.User.Identity;
        if (userIdentity is { IsAuthenticated: true, Name: { } }) 
            metadata.Add("User", userIdentity.Name);

        var callOptions = context.Options.WithHeaders(metadata);
        context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, callOptions);

        return base.AsyncUnaryCall(request, context, continuation);
    }
}