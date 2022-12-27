// enhance-your-grpc-client-logs-with-a-generic-logging-interceptor
// https://anthonygiretti.com/2022/08/08/net-6-enhance-your-grpc-client-logs-with-a-generic-logging-interceptor/

using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace GRPC.NET7.Client.Helpers;

public class TracerInterceptor : Interceptor
{
    private readonly ILogger<TracerInterceptor> _logger;

    public TracerInterceptor(ILoggerFactory logger)
    {
        _logger = logger.CreateLogger<TracerInterceptor>();
    }

    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        where TRequest : class
        where TResponse : class
    {
        _logger.LogDebug($"Calling {context.Method.Name} {context.Method.Type} method at {DateTime.UtcNow} UTC from machine {Environment.MachineName}");
        var continued = continuation(context);

        return continued;
    }

    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        where TRequest : class
        where TResponse : class
    {
        _logger.LogDebug($"Calling {context.Method.Name} {context.Method.Type} method at {DateTime.UtcNow} UTC from machine {Environment.MachineName}");
        var continued = continuation(context);

        return continued;
    }

    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        where TRequest : class
        where TResponse : class
    {
        _logger.LogDebug($"Calling {context.Method.Name} {context.Method.Type} method. Payload received: {request.GetType()} : {request}");
        _logger.LogDebug($"Calling {context.Method.Name} {context.Method.Type} method at {DateTime.UtcNow} UTC from machine {Environment.MachineName}");
        var continued = continuation(request, context);

        return continued;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        where TRequest : class
        where TResponse : class
    {
        _logger.LogDebug($"Calling {context.Method.Name} {context.Method.Type} method. Payload received: {request.GetType()} : {request}");
        _logger.LogDebug($"Calling {context.Method.Name} {context.Method.Type} method at {DateTime.UtcNow} UTC from machine {Environment.MachineName}");
        var continued = continuation(request, context);

        return continued;
    }
}