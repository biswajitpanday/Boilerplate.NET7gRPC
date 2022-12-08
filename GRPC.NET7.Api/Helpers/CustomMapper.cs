namespace GRPC.NET7.Api.Helpers;

public static class CustomMapper
{
    /// <summary>
    /// Proto to Entity/DTO And Reverse
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="srcValue"></param>
    /// <returns></returns>
    public static TDestination Map<TSource, TDestination>(TSource srcValue) where TDestination: new()
    {
        try
        {
            var destType = typeof(TDestination);
            var tDest = (TDestination)Activator.CreateInstance(destType)!;
            if (srcValue == null)
                return tDest;
            var srcClassType = srcValue.GetType();
            var srcProperties = srcClassType.GetProperties();
            foreach (var srcProperty in srcProperties)
            {
                var destPropertyInfo = tDest.GetType().GetProperty(srcProperty.Name);
                if (srcProperty.GetType() == destPropertyInfo?.GetType())
                    destPropertyInfo?.SetValue(tDest, srcProperty.GetValue(srcValue, null), null);
            }
            return tDest;
        }
        catch (Exception e)
        {
            throw new Exception($"Unsupported mapping.\n {e.Message}");
        }
    }
}