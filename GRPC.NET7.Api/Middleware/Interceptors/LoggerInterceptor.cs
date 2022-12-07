namespace GRPC.NET7.Api.Middleware.Interceptors;

public class LoggerInterceptor : Interceptor
{
    private readonly ILogger<LoggerInterceptor> _logger;

    public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
    {
        _logger = logger;
    }


    #region Server Side

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        LogCall(context);
        try
        {
            return await continuation(request, context);
        }
        catch (SqlException e)
        {
            _logger.LogError(e, $"An SQL error occurred when calling {context.Method}");
            Status status;

            if (e.Number == -2)
            {
                status = new Status(StatusCode.DeadlineExceeded, "SQL timeout");
            }
            else
            {
                status = new Status(StatusCode.Internal, "SQL error");
            }
            throw new RpcException(status);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"An error occurred when calling {context.Method}");
            throw new RpcException(Status.DefaultCancelled, e.Message);
        }
    }

    #endregion



    #region Client Side

    //public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
    //    TRequest request,
    //    ClientInterceptorContext<TRequest, TResponse> context,
    //    AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    //{
    //    LogCall(context.Method);

    //    var call = continuation(request, context);

    //    return new AsyncUnaryCall<TResponse>(HandleResponse(call.ResponseAsync), call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
    //}

    //private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> t)
    //{
    //    try
    //    {
    //        var response = await t;
    //        _logger.LogDebug($"Response received: {response}");
    //        return response;
    //    }
    //    catch (RpcException ex)
    //    {
    //        _logger.LogError($"Call error: {ex.Message}");
    //        return default;
    //    }
    //}


    //services.AddGrpcClient<CountryServiceClient>(o =>
    //{
    //    o.Address = new Uri("https://localhost:5001");
    //}).AddInterceptor<LoggerInterceptor>();
    #endregion



    #region Private Methods

    private void LogCall(ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        _logger.LogDebug($"Starting call. Request: {httpContext.Request.Path}");
    }

    private void LogCall<TRequest, TResponse>(Method<TRequest, TResponse> method) where TRequest : class where TResponse : class
    {
        _logger.LogDebug($"Starting call. Type: {method.Type}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
    }

    #endregion

}