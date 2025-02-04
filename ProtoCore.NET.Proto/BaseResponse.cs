using ProtoBuf;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ProtoCore.NET.Proto;

[ProtoContract]
public class BaseResponse<T>
{
    [ProtoMember(1)]
    public bool IsSuccess { get; set; }
    [ProtoMember(2)]
    public string? Message { get; set; }
    [ProtoMember(3)]
    public T? Data { get; set; }


    public static BaseResponse<T?> Ok(T? data, string? message = null)
    {
        return new BaseResponse<T?>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }

    public static BaseResponse<T> Created(T? data, string? message = null)
    {
        return new BaseResponse<T>
        {
            IsSuccess = true,
            Message = string.IsNullOrEmpty(message)
                ? ConstructMessage(GetCallingClassName())
                : message,
            Data = data
        };
    }

    public static BaseResponse<T> Updated(T? data, string? message = null)
    {
        return new BaseResponse<T>
        {
            IsSuccess = true,
            Message = string.IsNullOrEmpty(message)
                ? ConstructMessage(GetCallingClassName())
                : message,
            Data = data
        };
    }

    public static BaseResponse<T> Deleted(T? data, string? message = null)
    {
        return new BaseResponse<T>
        {
            IsSuccess = true,
            Message = string.IsNullOrEmpty(message)
                ? ConstructMessage(GetCallingClassName())
                : message,
            Data = data
        };
    }

    public static BaseResponse<T> Failed(T? data, string? message = null, [CallerMemberName] string actionName = "")
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Message = string.IsNullOrEmpty(message)
                ? ConstructMessage(GetCallingClassName(), false, actionName)
                : message,
            Data = data
        };
    }


    #region Private Methods

    private static string ConstructMessage(string callerName, bool isSuccess = true, [CallerMemberName] string actionName = "")
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
        catch (Exception)
        {
            return string.Empty;
        }
    }

    #endregion
}