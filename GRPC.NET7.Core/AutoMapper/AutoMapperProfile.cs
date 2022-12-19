using System.Reflection;
using AutoMapper;
using GRPC.NET7.Core.Interfaces.Common;

namespace GRPC.NET7.Core.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        var assemblies = GetListOfEntryAssemblyWithReferences();
        ApplyMappingsFromAssemblies(assemblies);
    }

    public List<Assembly> GetListOfEntryAssemblyWithReferences()
    {
        var listOfAssemblies = new List<Assembly>();
        var mainAsm = Assembly.GetEntryAssembly();
        if (mainAsm != null)
        {
            listOfAssemblies.Add(mainAsm);
            foreach (var refAsmName in mainAsm.GetReferencedAssemblies())
                listOfAssemblies.Add(Assembly.Load(refAsmName));
        }
        return listOfAssemblies.Where(a => a.FullName!.Contains("GRPC.NET7")).ToList();
    }

    private void ApplyMappingsFromAssemblies(List<Assembly> assemblies)
    {
        var types = assemblies
            .SelectMany(assembly => assembly.GetExportedTypes())
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToArray();

        foreach (var type in types)
        {
            var iMapType = typeof(IMapFrom<>);
            var mapperInvocationParam = new object[] { this };
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("Mapping");
            if (methodInfo != null)
                methodInfo.Invoke(instance, mapperInvocationParam);
            else
            {
                var interfaces = type
                    .GetInterfaces()
                    .Where(i => i.Name == iMapType.Name)
                    .ToArray();
                var methodInfos = interfaces
                    .Select(i => i.GetMethod("Mapping"))
                    .Where(m => m != null)
                    .ToArray();
                foreach (var mf in methodInfos)
                    mf?.Invoke(instance, mapperInvocationParam);
            }
        }
    }

    //private void ApplyMappingsFromAssemblies(List<Assembly> assemblies)
    //{
    //    var types = new List<Type>();
    //    foreach (var assembly in assemblies)
    //    {
    //        types.AddRange(assembly.GetExportedTypes()
    //            .Where(t => t.GetInterfaces()
    //                .Any(ct => ct.IsGenericType && ct.GetGenericTypeDefinition() == typeof(IMapFrom<>))).ToList());
    //    }

    //    foreach (var type in types)
    //    {
    //        var instance = Activator.CreateInstance(type);
    //        var methodInfo = type.GetMethod("Mapping")
    //                         ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");
    //        methodInfo?.Invoke(instance, new object?[] { this });
    //    }
    //}

}