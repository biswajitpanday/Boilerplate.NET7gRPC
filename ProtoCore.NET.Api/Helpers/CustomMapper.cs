namespace ProtoCore.NET.Api.Helpers;

public static class CustomMapper
{
    /// <summary>
    /// Proto to Entity/DTO And Reverse
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="src"></param>
    /// <returns>TDestination</returns>
    public static TDestination Map<TSource, TDestination>(TSource src) where TDestination : new()
    {
        try
        {
            var tDest = (TDestination)Activator.CreateInstance(typeof(TDestination))!;
            if (src == null)
                return tDest;
            var srcClassType = src.GetType();
            var srcProperties = srcClassType.GetProperties();
            foreach (var srcProperty in srcProperties)
            {
                var destPropertyInfo = tDest.GetType().GetProperty(srcProperty.Name);
                if (srcProperty.GetType() == destPropertyInfo?.GetType())
                    destPropertyInfo.SetValue(tDest, srcProperty.GetValue(src, null), null);
            }
            return tDest;
        }
        catch (Exception e)
        {
            throw new Exception($"Unsupported mapping.\n{e.Message}");
        }
    }
}