using System.Reflection;
using AutoMapper;
using GRPC.NET7.Core.Interfaces.Common;

namespace GRPC.NET7.Core.AutoMapper;

internal class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith(nameof(GRPC.NET7))).ToArray();
        ApplyMappingsFromAssemblies(assemblies);
    }

    private void ApplyMappingsFromAssemblies(IEnumerable<Assembly> assemblies)
    {
        var types = new List<Type>();
        foreach (var assembly in assemblies)
        {
            types.AddRange(assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces()
                    .Any(ct => ct.IsGenericType && ct.GetGenericTypeDefinition() == typeof(IMapFrom<>))).ToList());
        }

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("Mapping")
                             ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");
            methodInfo?.Invoke(instance, new object?[] { this });
        }
    }
}