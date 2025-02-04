using Newtonsoft.Json;
using ProtoCore.NET.Proto;

namespace ProtoCore.NET.Client.Helpers;

public static class ConsoleExtensions
{
    public static void Success(string data, bool disableNewLine = false)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        if (disableNewLine)
            Console.Write(data);
        else
            Console.WriteLine(data);
        Console.ResetColor();
    }

    public static void Error(string message, bool disableNewLine = false)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        if (disableNewLine)
            Console.Write(message);
        else
            Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void PrintResponse<T>(BaseResponse<T> response, Formatting formatting = Formatting.Indented) where T : class?
    {
        try
        {
            if (response.IsSuccess)
            {
                Success("------ Success Response ------");
                if (response.Data != null)
                {
                    Success("Response:");
                    Success(JsonConvert.SerializeObject(response, formatting: formatting));
                    foreach (var property in response.Data.GetType().GetProperties())
                    {
                        var value = property.GetValue(response.Data);
                        if (value != null)
                        {
                            Success($"{property.Name}: {value}");
                        }
                        else
                        {
                            Success($"{property.Name}: null");
                        }
                    }
                }
                else
                {
                    Success("No data available in the response.");
                }
            }
            else
            {
                // Print error response
                Error("------ Error Response ------");
                Error($"Message: {response.Message}");
            }
        }
        catch (Exception e)
        {
            // do nothing.
        }
        
    }
}
