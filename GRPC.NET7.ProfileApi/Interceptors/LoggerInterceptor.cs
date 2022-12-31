using Grpc.Core;
using Grpc.Core.Interceptors;

namespace GRPC.NET7.Profile.Api.Interceptors;

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
        //catch (SqlException e) //todo: Need to uncomment after necessary package install.
        //{
        //    _logger.LogError(e, $"An SQL error occurred when calling {context.Method}");
        //    Status status;

        //    if (e.Number == -2)
        //    {
        //        status = new Status(StatusCode.DeadlineExceeded, "SQL timeout");
        //    }
        //    else
        //    {
        //        status = new Status(StatusCode.Internal, "SQL error");
        //    }
        //    throw new RpcException(status);
        //}
        catch (Exception e)
        {
            _logger.LogError(e, $"An error occurred when calling {context.Method}");
            throw new RpcException(Status.DefaultCancelled, e.Message);
        }
    }

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