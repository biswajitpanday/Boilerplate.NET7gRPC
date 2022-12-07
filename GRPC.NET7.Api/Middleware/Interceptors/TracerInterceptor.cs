// For Client.
// enhance-your-grpc-client-logs-with-a-generic-logging-interceptor
// https://anthonygiretti.com/2022/08/08/net-6-enhance-your-grpc-client-logs-with-a-generic-logging-interceptor/

namespace GRPC.NET7.Api.Middleware.Interceptors;

public class TracerInterceptor : Interceptor
{
    private readonly ILogger<TracerInterceptor> _logger;

    public TracerInterceptor(ILogger<TracerInterceptor> logger)
    {
        _logger = logger;
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

// How to configure the Interceptor

//var builder = WebApplication.CreateBuilder(args);

//var loggerFactory = LoggerFactory.Create(logging =>
//{
//    logging.AddConsole();
//    logging.SetMinimumLevel(LogLevel.Trace);
//});

//builder.Services.AddGrpcClient<CountryServiceClient>(o =>
//    {
//        o.Address = new Uri("{gRpcServerBaseUri}");
//    })
//    .AddInterceptor(() => new TracerInterceptor(loggerFactory.CreateLogger<TracerInterceptor>()));

//var app = builder.Build();

//...

//app.Run();
