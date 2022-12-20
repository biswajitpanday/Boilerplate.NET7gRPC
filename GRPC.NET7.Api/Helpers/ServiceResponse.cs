using System.Diagnostics;
using System.Runtime.CompilerServices;
using GRPC.NET7.Proto.Protos;

namespace GRPC.NET7.Api.Helpers;

public static class ServiceResponse
{
    public static BaseResponse Created<T>(T? data, string? message = null) where T : class
    {
        return new BaseResponse
        {
            IsSuccess = true,
            Message = string.IsNullOrEmpty(message) 
                ? ConstructMessage(GetCallingClassName()) 
                : message,
            Data = JsonConvert.SerializeObject(data)
        };
    }

    public static BaseResponse Updated<T>(T? data, string? message = null) where T : class
    {
        return new BaseResponse
        {
            IsSuccess = true,
            Message = string.IsNullOrEmpty(message) 
                ? ConstructMessage(GetCallingClassName()) 
                : message,
            Data = JsonConvert.SerializeObject(data)
        };
    }

    public static BaseResponse Deleted<T>(T? data, string? message = null) where T : class
    {
        return new BaseResponse
        {
            IsSuccess = true,
            Message = string.IsNullOrEmpty(message) 
                ? ConstructMessage(GetCallingClassName()) 
                : message,
            Data = JsonConvert.SerializeObject(data)
        };
    }

    public static BaseResponse Failed<T>(T? data, string? message = null, [CallerMemberName] string actionName = "") where T : class
    {
        return new BaseResponse
        {
            IsSuccess = false,
            Message = string.IsNullOrEmpty(message) 
                ? ConstructMessage(GetCallingClassName(), false, actionName) 
                : message,
            Data = JsonConvert.SerializeObject(data)
        };
    }


    #region Private Methods

    private static string ConstructMessage(string callerName, bool isSuccess=true, [CallerMemberName] string actionName = "")
    {
        var messageStatus = isSuccess ? "Successfully" : "Failed";
        try
        {
            if (callerName.Contains("Service"))
            {
                var entityName = callerName[..^7];
                return $"{entityName} {actionName} {messageStatus}";
            }
            return $"{actionName} {messageStatus}";
        }
        catch (Exception)
        {
            // Ignore the exception...
            return $"{actionName} {messageStatus}";
        }
    }

    private static string GetCallingClassName()
    {
        string fullName;
        Type declaringType;
        var skipFrames = 2;
        try
        {
            do
            {
                var method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    return method.Name;
                }
                skipFrames++;
                fullName = declaringType.ReflectedType?.Name;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));
            return fullName;
        }
        catch (Exception )
        {
            return string.Empty;
        }
    }

    #endregion
}